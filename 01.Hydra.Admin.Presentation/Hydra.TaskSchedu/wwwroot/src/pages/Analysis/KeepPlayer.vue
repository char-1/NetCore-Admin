<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <div class="search-bar">
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
          style="width: 300px"></DatePicker>
          <Button type="ghost" @click="searchEvent"><i class="fa fa-search"></i>查询</Button>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button  :disabled="deleteDisabled" @click="deleteEvent"><i class="fa fa-trash"></i> 删除</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange" :border="border"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="searchModel.page" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
  </Row>
</template>
<script>
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm, FormatMoney } from "../../data/utils";
export default {
  data() {
    return {
      title: "玩家留存 (+N日留存率)",
      keyword: "",
      toolbar: false,
      deleteDisabled: true,
      tableLoading: true,
      total: 0,
      showNum: 15,
      showStripe: false,
      border: false,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      showColumns: [
        {
          title: "日期",
          key: "statTime",
          fixed: "left",
          width: 120,
          align: "center",
          render: (h, params) => {
            return this.Moment(params.row.statTime, "YYYY-MM-DD");
          }
        },
        {
          title: "用户数",
          key: "players",
          align: "center",
          render: (h, params) => {
            return FormatMoney(params.row.players);
          }
        },
        {
          title: "+1 日",
          key: "secondDay",
          align: "center",
          render: (h, params) => {
            return params.row.secondDay == 0
              ? "N/A"
              : (params.row.secondDay * 100).toFixed(2) + "%";
          }
        },
        {
          title: "+3 日",
          key: "thirdDay",
          align: "center",
          render: (h, params) => {
            return params.row.thirdDay == 0
              ? "N/A"
              : (params.row.thirdDay * 100).toFixed(2) + "%";
          }
        },
        {
          title: "+7 日",
          key: "seventhDay",
          align: "center",
          render: (h, params) => {
            return params.row.seventhDay == 0
              ? "N/A"
              : (params.row.seventhDay * 100).toFixed(2) + "%";
          }
        },
        {
          title: "+15 日",
          key: "fifteenDay",
          align: "center",
          render: (h, params) => {
            return params.row.fifteenDay == 0
              ? "N/A"
              : (params.row.fifteenDay * 100).toFixed(2) + "%";
          }
        },
        {
          title: "+30 日",
          key: "thirtiethDay",
          align: "center",
          render: (h, params) => {
            return params.row.thirtiethDay == 0
              ? "N/A"
              : (params.row.thirtiethDay * 100).toFixed(2) + "%";
          }
        }
      ],
      searchModel: {
        stime: this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        etime: this.moment().format("YYYY-MM-DD"),
        page: 1,
        times: [
          this.moment()
            .add(-7, "days")
            .format("YYYY-MM-DD"),
          this.moment().format("YYYY-MM-DD")
        ]
      },
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
        ]
      },
      disabledDate: date => {
        return date.getTime() > Date.now();
      }
    };
  },
  methods: {
    searchEvent: function() {
      this.searchModel.page = 1;
      let diffDays = moment(this.searchModel.etime).diff(
        this.searchModel.stime,
        "days"
      );
      if (diffDays > 30) {
        this.$Notice.warning({
          title: "只提供30天内数据查询"
        });
        return;
      }
      this.initTableData();
    },
    getDatepicker: function(times) {
      this.searchModel.stime = times[0];
      this.searchModel.etime = times[1];
    },
    pageChange: function(page) {
      this.searchModel.page = page;
      this.initTableData();
    },
    selectChange: function(data) {
      this.dataSelect = data;
      this.deleteDisabled = false;
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_REMAIN_GRID, {
        p: this.searchModel.page,
        stime: this.searchModel.stime,
        etime: this.searchModel.etime,
        size: this.showNum
      })
        .then(result => {
          if (result && result.data.data.rows.length > 0) {
            this.dataShow = result.data.data.rows;
            this.total = result.data.data.total;
          } else {
            this.dataShow = [];
            this.total = 0;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 800);
        });
    },
    Moment(date, format) {
      return moment(date).format(format);
    },
    deleteEvent: function() {
      if (this.dataSelect && this.dataSelect.length > 0) {
        let ids = this._.map(this.dataSelect, "id");
        this.$Modal.confirm({
          title: "系统提示",
          content: "确定删除选择项?",
          onOk: () => {
            HttpPost(
              HTTP_URL_API.DELETE_LOGS_INFO,
              SerializeForm({ logIds: ids.join(",") })
            ).then(result => {
              if (result && result.data.state == "success") {
                this.$Message.success("提交成功");
                setTimeout(() => {
                  this.initTableData(1);
                }, 500);
              } else this.$Message.warning(result.data.message);
            });
          }
        });
      }
    }
  },
  mounted: function() {
    this.initTableData(1);
  },
  watch: {
    dataSelect: function() {
      this.deleteDisabled = this.dataSelect.length === 0;
    }
  }
};
</script>