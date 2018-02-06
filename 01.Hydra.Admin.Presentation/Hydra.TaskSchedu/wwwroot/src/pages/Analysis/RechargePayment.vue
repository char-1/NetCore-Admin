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
        <Tabs :value="chartTitle" size="small" @on-click="tabsClickEvent" :animated="false">
            <TabPane v-for="item in Tabs" :label="item.name" :key="item.id" :name="item.name">
                <VmEChartLine :title="chartTitle" :xAxisData="chartLine.xAxisData" :series="chartLine.series" v-on:chartClickEvent="chartClickEvent"/>
            </TabPane>
            <div slot="extra" class="ivu-tabs-nav-right-ext">合计:{{FormatMoney(TabExtTotal)}}</div>
        </Tabs>
      </Row>
      <Row style="margin-top:3px;">
        <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
        <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" :styles="pageStyles" show-total></Page>
      </Row>  
    </div>
    <Modal
        v-model="modalShow"
        :title="chartTitle"
        :mask-closable="false"
        :transfer="true"
        :scrollable="true"
        @on-cancel="cancelEvent"
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
import moment from "moment";
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
      title: "充值收入",
      pageStyles: {
        "margin-top": "10px",
        float: "left"
      },
      Tabs: [
        { id: 0, name: "充值金额" },
        { id: 1, name: "充值次数" },
        { id: 2, name: "充值人数" }
      ],
      TabExtTotal: 0,
      chartTitle: "充值金额",
      chartType: 0, //图表类型
      pointDate: "",
      pointType: 1,
      open: false,
      dateRange: "",
      dashData: {}, //数据集合
      disabledPicker: true,
      searchModel: {
        times: [
          this.moment()
            .add(-7, "days")
            .format("YYYY-MM-DD"),
          this.moment().format("YYYY-MM-DD")
        ],
        modalTimes: [
          this.moment()
            .add(-7, "days")
            .format("YYYY-MM-DD"),
          this.moment().format("YYYY-MM-DD")
        ],
        accountId: ""
      },
      sdate: this.moment()
        .add(-7, "days")
        .format("YYYY-MM-DD"),
      edate: this.moment().format("YYYY-MM-DD"),
      dashboardItem: [0, 0, 0, 0],
      currentTabs: 0,
      firstLoad: true,
      chartLine: {
        xAxisData: [],
        series: []
      },
      total: 0,
      showNum: 5,
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
          title: "充值金额",
          key: "number",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.number);
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
      //modal Table
      pickerPlaceholder: "查询时间",
      modalShow: false,
      modelDatas: [],
      modalColumns: [],
      modalTotal: 0,
      modalPageSize: 5,
      modalCurrentPage: 1,
      gameSelect: [],
      modalType: 99998,
      modalAccountId: "",
      modalToday: ""
    };
  },
  methods: {
    pickerSearchEvent: function(times) {},
    modalSearchEvent: function() {
      this.initTodayDetail();
    },
    modalPageChangeEvant: function(page) {
      this.modalCurrentPage = page;
      this.initTodayDetail();
    },
    searchEvent() {
      this.open = false;
      if (this.sdate && this.edate) {
        let diffDays = this.moment(this.edate).diff(this.sdate, "days");
        if (diffDays > 30) {
          this.$Notice.warning({
            title: "只提供30天内数据查询"
          });
          return;
        }
      }
      this.chartTitle = "充值金额";
      this.chartType = 0;
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
      HttpGet(HTTP_URL_API.GET_PLAT_RECHARGE, {
        stime: this.sdate,
        etime: this.edate,
        goleType: this.chartType,
        tabText: this.chartTitle
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
            this.TabExtTotal = result.data.data.tabExt;
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
    chartClickEvent: function(params) {
      if (params.seriesType != "bar") {
        this.modalCurrentPage = 1;
        this.modalToday = params.name;
        this.initTodayDetail();
      }
    },
    tabsClickEvent: function(name) {
      if (name) {
        this.chartTitle = name;
        this.chartType = this._.find(this.Tabs, { name: name }).id;
        this.showColumns[1].title = name;
        this.initData();
      }
    },
    initTodayDetail: function() {
      this.modalShow = true;
      this.modalColumns = [
        {
          title: "玩家ID",
          key: "accountId"
        },
        {
          title: "充值金币",
          key: "gold",
          render: (h, params) => {
            return FormatMoney(params.row.gold);
          }
        },
        {
          title: "备注",
          key: "remark"
        },
        {
          title: "操作时间",
          key: "createTime",
          width: 180,
          render: (h, params) => {
            return this.Moment(params.row.createTime, "YYYY-MM-DD HH:mm:ss");
          }
        }
      ];
      HttpGet(HTTP_URL_API.GET_PLAYER_GOLD, {
        stime: this.modalToday,
        etime: this.modalToday,
        p: this.modalCurrentPage,
        size: this.modalPageSize,
        goleType: 1,
        accountId: this.searchModel.accountId
      }).then(result => {
        if (result.data && result.data.data.rows.length > 0) {
          this.modelDatas = result.data.data.rows;
          this.modalTotal = result.data.data.total;
        } else {
          this.modelDatas = [];
          this.modalTotal = 0;
        }
      });
    },
    cancelEvent: function() {
      this.searchModel.accountId = "";
    },
    Moment(date, format) {
      return moment(date).format(format);
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
.ivu-tabs-nav-right-ext {
  padding-top: 5px;
  font-size: 14px;
  font-weight: bold;
}
</style>
