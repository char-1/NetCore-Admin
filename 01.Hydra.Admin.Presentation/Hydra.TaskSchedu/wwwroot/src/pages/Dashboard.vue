<template>
  <div class="dashboard">
    <Row type="flex" justify="end" style="margin-top:8px;margin-bottom:8px;">
        <DatePicker 
            type="daterange" 
            :editable="false"
            :open="open"
            :options="searchOptions"
            placement="bottom-end" 
            format="yyyy-MM-dd"
            :transfer="true"
            confirm
            :clearable="false"
            @on-change='getDatepicker'
            @on-clear="handleClear"
            @on-ok="handleOk"
            style="margin-bottom:5px;border:none;">
              <a href="javascript:void(0)" @click="handleClick" style="font-weight: bolder;color:#2b83f9;">
                  <Icon type="calendar" style="font-weight: bolder;"></Icon>&nbsp;&nbsp;
                  <template v-if="dateRange === ''">{{sdate}} ~ {{edate}}</template>
                  <template v-else>{{ dateRange }}</template>
                  &nbsp;&nbsp;<Icon type="ios-arrow-down" style="font-weight: bolder;"></Icon>                          
              </a>            
            </DatePicker>
    </Row>
    <Row :gutter="16">
      <Col :lg="6" :md="12">
        <VmStateOverView color="#41b883" icon="ivu-icon ivu-icon-person-add" title="新增玩家数/人" :count="FormatMoney(dashboardItem[0])" tooltip="所选时间段内新增玩家数"></VmStateOverView>
      </Col>
      <Col :lg="6" :md="12">
        <VmStateOverView color="#1d8ce0" icon="ivu-icon ivu-icon-ios-loop-strong" title="充值玩家数/人" :count="FormatMoney(dashboardItem[1])" tooltip="所选时间段内充值玩家数"></VmStateOverView>
      </Col>
      <Col :lg="6" :md="12">
        <VmStateOverView color="#ffa000" icon="ivu-icon ivu-icon-social-usd" title="充值金额/元" :count="FormatMoney(dashboardItem[2])"  tooltip="所选时间段内充值金额"></VmStateOverView>
      </Col>
      <Col :lg="6" :md="12">
        <VmStateOverView color="#f60000" icon="ivu-icon ivu-icon-ios-people-outline" title="绑定玩家数/人" :count="FormatMoney(dashboardItem[3])" tooltip="所选时间段内绑定玩家数"></VmStateOverView>
      </Col>
    </Row>
    <Row type="flex" justify="start" style="margin-top:15px;margin-bottom:5px;">
      <label><strong style="font-size:13px;">数据趋势</strong></label>
    </Row>
    <Row style="margin-top:8px;">
      <Tabs value="0" size="small" @on-click="tabsClickEvent">
            <TabPane label="新增玩家" name="0">
              <VmEChartLine title="" :xAxisData="chartLine.xAxisData" :series="chartLine.series">
              </VmEChartLine>
            </TabPane>
            <TabPane label="充值玩家" name="1">
              <VmEChartLine title="" :xAxisData="chartLine1.xAxisData" :series="chartLine1.series">
              </VmEChartLine>
            </TabPane>
            <TabPane label="充值金额" name="2">
              <VmEChartLine title="" :xAxisData="chartLine2.xAxisData" :series="chartLine2.series">
              </VmEChartLine>
            </TabPane>
            <TabPane label="绑定玩家" name="3">
              <VmEChartLine title="" :xAxisData="chartLine3.xAxisData" :series="chartLine3.series">
              </VmEChartLine>
            </TabPane>
      </Tabs>
    </Row>
    <Row type="flex" justify="start" style="margin-top:15px;margin-bottom:5px;">
      <label><strong style="font-size:13px;">数据详情</strong></label>
    </Row>
    <Row style="margin-top:8px;">
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
      <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" :styles="pageStyles" show-total></Page>
    </Row>  
  </div>
</template>

<script>
import VmEChartLine from "@/components/vm-echart-line";
import VmStateOverView from "@/components/vm-state-overview.vue";
import { HTTP_URL_API } from "../data/api";
import {HttpGet,HttpPost,SerializeForm,FormatMoney} from "../data/utils";
export default {
  name: "Dashboard",
  components: {
    VmStateOverView,
    VmEChartLine
  },
  data() {
    return {
      pageStyles: {
        "margin-top": "10px",
        float: "left"
      },
      open: false,
      dateRange: "",
      dashData: {}, //数据集合
      sdate: this.moment()
        .add(-7, "days")
        .format("YYYY-MM-DD"),
      edate: this.moment().format("YYYY-MM-DD"),
      dashboardItem: [0, 0, 0, 0],
      currentTabs: 0,
      firstLoad: true,
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
          },
          {
            text: "最近三月",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              return [start, end];
            }
          }
        ],
        disabledDate: date => {
          return date.getTime() > Date.now();
        }
      },
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
            data: []
          }
        ]
      },
      chartLine1: {
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
            data: []
          }
        ]
      },
      chartLine2: {
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
            data: []
          }
        ]
      },
      chartLine3: {
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
            data: []
          }
        ]
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
          title: "日期",
          key: "days",
          ellipsis: true
        },
        {
          title: "新增玩家",
          key: "addPlayers",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.addPlayers);
          }
        },
        {
          title: "充值玩家",
          key: "rechargePlayers",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.rechargePlayers);
          }
        },
        {
          title: "充值金额",
          key: "rechargeAmount",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.rechargeAmount);
          }
        },
        {
          title: "绑定玩家",
          key: "bindPlayers",
          ellipsis: true,
          sortable: true,
          render: (h, params) => {
            return FormatMoney(params.row.bindPlayers);
          }
        }
      ]
    };
  },
  methods: {
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
    handleOk() {
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
      this.initData();
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
      HttpGet(HTTP_URL_API.GET_DASHBOARD_CHART, {
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
            this.chartLine1.xAxisData = result.data.data.xAxisData;
            this.chartLine2.xAxisData = result.data.data.xAxisData;
            this.chartLine3.xAxisData = result.data.data.xAxisData;
            this.chartLine.series[0].data =
              result.data.data.yAxisData.addPlayers;
            this.chartLine1.series[0].data =
              result.data.data.yAxisData.rechargePlayers;
            this.chartLine2.series[0].data =
              result.data.data.yAxisData.rechargeAmount;
            this.chartLine3.series[0].data =
              result.data.data.yAxisData.bindPlayers;
            this.dashboardItem = result.data.data.dashBoardItem;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 300);
        });
    },
    tabsClickEvent: function(name) {
      this.currentTabs = parseInt(name);
      this.chartLine.series[0].data = this.dashData.yAxisData.addPlayers;
      this.chartLine1.series[0].data = this.dashData.yAxisData.rechargePlayers;
      this.chartLine2.series[0].data = this.dashData.yAxisData.rechargeAmount;
      this.chartLine3.series[0].data = this.dashData.yAxisData.bindPlayers;
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
