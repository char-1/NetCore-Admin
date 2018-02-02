<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <div class="search-bar">
        <Row type="flex" :gutter="16">
          <Col>
            <DatePicker 
            type="daterange" 
            :editable="false"
            :options="searchOptions"
            :clearable="false"
            placement="bottom-start" 
            placeholder="开始时间-结束时间" 
            v-model="searchModel.times" 
            format="yyyy-MM-dd"
            @on-change='getDatepicker'
            style="width: 200px"></DatePicker>
          </Col>
          <Col>
              <Button type="ghost" @click="searchEvent"><i class="fa fa-search">查询</i></Button>
          </Col>
        </Row>          
        </div>
      </Row>
      <Row style="margin-top:3px;border-top:1px dashed #eeeff1">
        <VmEChartLine title="" :xAxisData="chartLine.xAxisData" :series="chartLine.series"/>
      </Row>
      <Row style="margin-top:3px;">
        <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
        <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" :styles="pageStyles" show-total></Page>
      </Row>  
    </div>
    <Modal
        v-model="modalShow"
        :title="modalTitle"
        :mask-closable="false"
        :transfer="true"
        :scrollable="true"
        width="900">
        <VmModalTable 
        :modalColumns="modalColumns"
        :modalTotal="modalTotal"
        :gameSelect="gameSelect"
        :modalType="modalType"
        :searchOptions="searchOptions"
        :searchModel="searchModel"
        :modalPageSize="modalPageSize"
        :modalCurrentPage="modalCurrentPage"
        :modalDatas="modelDatas"
        v-on:modalPageChangeEvant="modalPageChangeEvant"
        v-on:pickerSearchEvent="pickerSearchEvent"
        v-on:modalSearchEvent="modalSearchEvent"></VmModalTable>
    </Modal>    
</Row>
</template>
<script>
import VmEChartLine from "@/components/vm-echart-line";
import VmModalTable from "@/components/vm-table-modal";
import { HTTP_URL_API } from "../../data/api";
import {
  HttpGet,
  HttpPost,
  SerializeForm,
  FormatMoney
} from "../../data/utils";
export default {
  components: {
    VmEChartLine,
    VmModalTable
  },
  data() {
    return {
      title: "平台收支",
      pageStyles: {
        "margin-top": "10px",
        float: "left"
      },
      gameSelect: [
        { value: "0", key: "全部" },
        { value: "20011", key: "金鲨银鲨" },
        { value: "10021", key: "百人金花" },
        { value: "10031", key: "百人牛牛" },
        { value: "30011", key: "走地德州" }
      ],
      open: false,
      dateRange: "",
      dashData: {}, //数据集合
      disabledPicker: true,
      searchModel: {
        times: [
          this.moment().add(-7, "days").format("YYYY-MM-DD"),
          this.moment().format("YYYY-MM-DD")
        ],
        gameId: 0
      },
      sdate: this.moment().add(-7, "days").format("YYYY-MM-DD"),
      edate: this.moment().format("YYYY-MM-DD"),
      dashboardItem: [0, 0, 0, 0],
      currentTabs: 0,
      firstLoad: true,
      chartLine: {
        xAxisData: [],
        series: []
      },
      total: 0,
      showNum: 10,
      currentPage: 1,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      tableLoading: false,
      showColumns: [
        {
          title: "时间段",
          key: "primaryKey",
          ellipsis: true
        },
        {
          title: "收入",
          key: "positive",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.positive);
          }
        },
        {
          title: "支出",
          key: "negative",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.negative);
          }
        },
        {
          title: "实际",
          key: "",
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.negative + params.row.positive);
          }
        }
      ],
      searchOptions: {
        shortcuts: [
          {
            text: "最近一周",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              return [start, end];
            }
          },
          {
            text: "最近一月",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              return [start, end];
            }
          }
        ],
        disabledDate: date => {
          return date.getTime() > Date.now();
        }
      },
      modalTitle: "收支明细",
      modalShow: false,
      modalType: -99999,
      modelDatas: [],
      modalColumns: [],
      modalTotal: 0,
      modalPageSize: 5,
      modalCurrentPage: 1,
      modalToday: ""
    };
  },
  methods: {
    pickerSearchEvent: function() {},
    modalPageChangeEvant: function(page) {
      this.modalCurrentPage = page;
      this.initModalTable(this.modalToday);
    },
    modalSearchEvent: function() {
      this.initModalTable(this.modalToday);
    },
    searchEvent() {
      this.open = false;
      if (this.sdate && this.edate && this.searchModel.tabs == 1) {
        let diffDays = this.moment(this.edate).diff(this.sdate, "days");
        if (diffDays > 30) {
          this.$Notice.warning({
            title: "只提供30天内数据查询"
          });
          return;
        }
      }
      this.initData();
    },
    handleClick() {
      this.open = !this.open;
    },
    handleClear() {
      this.open = false;
    },
    getDatepicker: function(times) {
      this.sdate = times[0];
      this.edate = times[1];
    },
    FormatMoney(number) {
      return FormatMoney(number);
    },
    pageChange: function(page) {
      this.currentPage = page;
      let startPage = (this.currentPage - 1) * this.showNum;
      let endPage = startPage + this.showNum;
      this.dataShow = this.dashData.table.rows.slice(startPage, endPage);
    },
    selectChange: function(data) {
      this.dataSelect = data;
    },
    initData: function() {
      this.dataShow = [];
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_GAME_PROFIT, {
        stime: this.sdate,
        etime: this.edate
      })
        .then(result => {
          if (result && result.data.data) {
            this.dashData = result.data.data;
            let startPage = (this.currentPage - 1) * this.showNum;
            let endPage = startPage + this.showNum;
            this.dataShow = result.data.data.table.rows.slice(
              startPage,
              endPage
            );
            this.total = result.data.data.table.total;
            this.chartLine.xAxisData = result.data.data.eChart.xAxis;
            this.chartLine.series = result.data.data.eChart.series;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 300);
        });
    },
    initModalTable: function(_today) {
      this.modalShow = true;
      this.modelDatas = [];
      this.modalTotal = 0;
      HttpGet(HTTP_URL_API.GET_PLAYER_RELATED, {
        gameId: this.searchModel.gameId
      }).then(result => {
        if (result && result.data.data.list.length > 0) {
          this.modelDatas = result.data.data.list;
          this.modalTotal = result.data.data.count;
        }
      });
    },
    tableShowDetail: function(today) {
      this.modalToday = today;
      this.modalColumns = [
        {
          title: "游戏名称",
          key: "gameId"
        },
        {
          title: "金币数",
          key: "gold",
          render: (h, params) => {
            return FormatMoney(params.row.gold);
          }
        },
        {
          title: "类型",
          key: "paymentType"
        }
      ];
      this.initModalTable(today);
    }
  },
  mounted: function() {
    this.initData();
  }
};
</script>
<style scoped>
.ivu-tabs-bar {
  border: none !important;
}
</style>
