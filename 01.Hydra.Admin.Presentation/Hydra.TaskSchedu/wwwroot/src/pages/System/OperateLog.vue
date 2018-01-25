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
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
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
import { HttpPost, HttpGet, SerializeForm } from "../../data/utils";
export default {
  data() {
    return {
      title: "操作记录",
      keyword: "",
      toolbar: true,
      deleteDisabled: true,
      tableLoading: true,
      total: 0,
      showNum: 15,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      showColumns: [
        {
          type: "selection",
          width: 60,
          align: "center"
        },
        {
          title: "操作人",
          key: "user"
        },
        {
          title: "操作IP",
          key: "ip",
          ellipsis: true
        },
        {
          title: "操作描述",
          key: "types",
          ellipsis: true
        },
        {
          title: "Controller",
          key: "controller",
          ellipsis: true
        },
        {
          title: "Action",
          key: "action",
          ellipsis: true
        },
        {
          title: "操作内容",
          key: "remark",
          ellipsis: true
        },
        {
          title: "操作时间",
          key: "createTime",
          render: (h, params) => {
            return this.Moment(params.row.createTime, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "操作",
          key: "action",
          width: 70,
          align: "center",
          render: (h, params) => {
            return h("div", [
              h(
                "Button",
                {
                  props: {
                    type: "success",
                    size: "small"
                  },
                  style: {
                    marginRight: "2px"
                  },
                  on: {
                    click: () => {
                      this.$Modal.info({
                        title: "操作日志",
                        content: `操作人：${this.dataShow[params.index]
                          .user}<br>操作时间：${this.dataShow[params.index]
                          .createTime}<br>操作记录：${this.dataShow[params.index]
                          .remark}<br>操作IP：${this.dataShow[params.index].ip}`
                      });
                    }
                  }
                },
                "查看"
              )
            ]);
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
      HttpGet(HTTP_URL_API.GET_LOGS_LIST, {
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