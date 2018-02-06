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
          placement="bottom-start" 
          placeholder="开始时间-结束时间" 
          v-model="searchModel.times" 
          format="yyyy-MM-dd"
          @on-change='getDatepicker'
          style="width: 250px"></DatePicker>
          <Button type="ghost" @click="searchEvent"><i class="fa fa-search"></i>查询</Button>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button type="primary" @click="exportCsvEvent"><Icon type="ios-download-outline"></Icon> 导出</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="searchModel.page" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
    <Modal
        v-model="modalAdopt"
        title="系统提示"
        ok-text="确定"
        cancel-text="取消"
        @on-ok="confirmOkEvent">
        {{modalConfirmText}}
    </Modal>
  </Row>
</template>
<script>
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import { HttpGet, HttpPost, SerializeForm, MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "意见建议",
      keyword: "",
      toolbar: true,
      modalConfirm: false,
      adoptDisabled: true,
      tableLoading: true,
      modalConfirmText: "确定采纳选中数据?",
      modalAdopt: false,
      total: 0,
      showNum: 15,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      showColumns: [
        // {
        //   type: "index",
        //   width: 60,
        //   align: "center"
        // },
        {
          title: "昵称",
          key: "name"
        },
        {
          title: "问题",
          key: "text",
          ellipsis: true
        },
        {
          title: "状态",
          key: "state",
          render: (h, params) => {
            switch (params.row.state) {
              case 0:
                return "未采纳";
              case 1:
                return "已采纳";
            }
          }
        },
        {
          title: "时间",
          key: "time",
          render: (h, params) => {
            return this.Moment(params.row.time, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "操作",
          key: "action",
          width: 220,
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
                        title: "意见建议",
                        content: `意见状态：${
                          this.dataShow[params.index].state == 0
                            ? "未采纳"
                            : "已采纳"
                        }<br>问题描述：${
                          this.dataShow[params.index].text
                        }<br>会员昵称：${this.dataShow[params.index].name}`
                      });
                    }
                  }
                },
                "查看"
              ),
              h(
                "Button",
                {
                  props: {
                    type: "warning",
                    size: "small"
                  },
                  style: {
                    marginRight: "5px"
                  },
                  on: {
                    click: () => {
                      this.modalAdopt = true;
                      this.dataSelect = [];
                      this.dataSelect.push(
                        new Array(params.index, params.row._id)
                      );
                    }
                  }
                },
                "采纳"
              ),
              h(
                "Button",
                {
                  props: {
                    type: "error",
                    size: "small"
                  },
                  on: {
                    click: () => {
                      this.$Modal.confirm({
                        title: "系统提示",
                        content: "确定删除选择项?",
                        onOk: () => {
                          HttpPost(
                            HTTP_URL_API.REM_BUG_INFO,
                            MakeSign({
                              id: params.row._id
                            }),
                            "application/json;charset=utf-8"
                          ).then(result => {
                            if (result && result.data.data == "success") {
                              this.$Message.success("操作成功");
                              setTimeout(() => {
                                this.dataShow.splice(params.index, 1);
                                this.total -= 1;
                              }, 500);
                            } else this.$Message.error("操作失败");
                          });
                        }
                      });
                    }
                  }
                },
                "删除"
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
      }
    };
  },
  methods: {
    searchEvent: function() {
      this.searchModel.page = 1;
      this.initTableData();
    },
    getDatepicker: function(times) {
      this.searchModel.stime = times[0];
      this.searchModel.etime = times[1];
    },
    confirmOkEvent: function() {
      if (this.dataSelect) {
        let data = { id: this.dataSelect[0][1] };
        HttpPost(
          HTTP_URL_API.SET_BUG_LIST,
          MakeSign(data),
          "application/json;charset=utf-8"
        ).then(result => {
          if (result && result.data.data == "success") {
            this.modalAdopt = false;
            this.$Notice.success({
              title: "操作成功"
            });
            this.dataSelect = [];
          } else this.$Message.error("操作失败");
        });
      }
    },
    pageChange: function(page) {
      this.searchModel.page = page;
      this.initTableData();
    },
    selectChange: function(data) {
      this.dataSelect = data;
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_BUG_LIST, {
        p: this.searchModel.page,
        stime: this.searchModel.stime,
        etime: this.searchModel.etime,
        size: this.showNum
      })
        .then(result => {
          if (result && result.data.data.list.length > 0) {
            this.dataShow = result.data.data.list;
            this.total = result.data.data.count;
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
    exportCsvEvent: function() {
      if (this.dataShow.length > 0)
        this.$refs.selection.exportCsv({
          filename: "意见建议列表",
          columns: this.showColumns.filter((col, index) => index <= 3)
        });
      else
        this.$Notice.warning({
          title: "导出数据为空"
        });
    }
  },
  mounted: function() {
    this.initTableData(1);
  },
  watch: {
    dataSelect: function() {
      if (this.dataSelect.length === 0) {
        this.adoptDisabled = true;
      } else {
        this.adoptDisabled = false;
      }
    }
  }
};
</script>