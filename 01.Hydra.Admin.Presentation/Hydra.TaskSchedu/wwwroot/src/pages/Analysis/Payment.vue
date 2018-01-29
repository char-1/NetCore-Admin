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
            <RadioGroup v-model="searchModel.dateTabs" type="button" @on-change="radioChangeEvent">
                <Radio label="小时"></Radio>
                <Radio label="日期"></Radio>
            </RadioGroup>
          </Col>
          <Col>
            <DatePicker 
            type="daterange" 
            :editable="false"
            :options="searchOptions"
            :clearable="false"
            :disabled="disabledPicker"
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
        <VmEChartLine title="收入/支出" :xAxisData="chartLine.xAxisData" :series="chartLine.series"/>
      </Row>
      <Row style="margin-top:3px;">
        <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
        <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" :styles="pageStyles" show-total></Page>
      </Row>  
    </div>
</Row>
</template>
<script>
import VmEChartLine from "@/components/vm-echart-line";
import { HTTP_URL_API } from "../../data/api";
import {
  HttpGet,
  HttpPost,
  SerializeForm,
  FormatMoney
} from "../../data/utils";
export default {
  name: "Dashboard",
  components: {
    VmEChartLine
  },
  data() {
    return {
      title: "平台收支",
      pageStyles: {
        "margin-top": "10px",
        float: "left"
      },
      open: false,
      dateRange: "",
      dashData: {}, //数据集合
      disabledPicker: true,
      searchModel: {
        times: [
          this.moment().format("YYYY-MM-DD"),
          this.moment().format("YYYY-MM-DD")
        ],
        dateTabs: "小时",
        tabs: 0
      },
      sdate: this.moment().format("YYYY-MM-DD"),
      edate: this.moment().format("YYYY-MM-DD"),
      dashboardItem: [0, 0, 0, 0],
      currentTabs: 0,
      firstLoad: true,
      chartLine: {
        xAxisData: [],
        series: [
          {
            type: "line",
            smooth: true,
            itemStyle: {
              normal: {
                color: "#2b83f9",
                areaStyle: {
                  color: "#E8F5FD"
                }
              }
            },
            name: "收入",
            data: []
          },
          {
            type: "line",
            smooth: true,
            itemStyle: {
              normal: {
                color: "#6a7985",
                areaStyle: {
                  color: "red"
                }
              }
            },
            name: "支出",
            data: []
          }
        ]
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
          key: "rowKey",
          ellipsis: true
        },
        {
          title: "收入",
          key: "rowValue",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.rowValue);
          }
        },
        {
          title: "支出",
          key: "rowValue",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.rowValue);
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
      }
    };
  },
  methods: {
    radioChangeEvent(data) {
      this.disabledPicker = data == "小时";
      this.searchModel.tabs = data == "小时" ? 0 : 1;
      let s = this.moment().format("YYYY-MM-DD");
      this.searchModel.times = [s, s];
      this.sdate = s;
      this.edate = s;
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
      HttpGet(HTTP_URL_API.GET_PLAYER_ONLINE, {
        tabs: this.searchModel.tabs,
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
            this.chartLine.xAxisData = result.data.data.xAxisData;
            this.chartLine.series[0].data = result.data.data.yAxisData;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 300);
        });
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
