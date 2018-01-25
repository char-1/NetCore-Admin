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
              <Input placeholder="玩家昵称/ID" v-model="searchModel.name" style="width: 200px"/>
          </Col>
          <Col>
              <Select v-model="searchModel.frozen" placeholder="玩家状态">
                  <Option v-for="item in playerFreezeObject" :value="item.value" :key="item.key">
                      {{ item.key }}
                  </Option>
              </Select>
          </Col>
          <Col>
              <Select v-model="searchModel.playerType" placeholder="玩家类型">
                  <Option v-for="item in playerTypeObject" :value="item.value" :key="item.key">
                      {{ item.key }}
                  </Option>
              </Select>
          </Col>          
          <Col>
              <Button type="ghost" @click="searchEvent"><i class="fa fa-search">查询</i></Button>
          </Col>
        </Row>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button  :disabled="adoptDisabled"><i class="fa fa-plus"></i> 删除</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
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
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import {
  HttpGet,
  HttpPost,
  SerializeForm,
  FormatMoney,
  MakeSign
} from "../../data/utils";
import VmExpandBets from "../../components/vm-table-expand-bets";
import VmModalTable from "../../components/vm-table-modal";
export default {
  data() {
    return {
      title: "所有玩家",
      keyword: "",
      toolbar: false,
      modalTitle: "我的充值",
      modalShow: false,
      adoptDisabled: true,
      tableLoading: true,
      playerFreezeObject: [
        { key: "全部", value: -1 },
        { key: "正常", value: 0 },
        { key: "锁定", value: 1 }
      ],
      playerTypeObject: [
        { key: "全部", value: -2 },
        { key: "机器人", value: -1 },
        { key: "游客", value: 0 },
        { key: "正式", value: 1 }
      ],
      gameSelect: [
        { value: "", key: "全部" },
        { value: "20011", key: "金鲨银鲨" },
        { value: "10021", key: "百人金花" },
        { value: "10031", key: "百人牛牛" },
        { value: "30011", key: "走地德州" }
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
        ]
      },
      total: 0,
      showNum: 20,
      currentPage: 1,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      goldNumber: 100, //上下分值
      goldType: 1, //上分(-1,下分),
      playerVIP: 0,
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "昵称",
          key: "name",
          ellipsis: true,
          render: (h, params) => {
            return h(
              "Poptip",
              {
                props: {
                  trigger: "hover",
                  title: "玩家信息",
                  placement: "bottom"
                }
              },
              [
                h("p", params.row.name),
                h(
                  "div",
                  {
                    slot: "title"
                  },
                  [
                    h(
                      "p",
                      {
                        style: {
                          fontWeight: "bold"
                        }
                      },
                      "玩家信息"
                    )
                  ]
                ),
                h(
                  "div",
                  {
                    slot: "content"
                  },
                  [
                    h(
                      "Button",
                      {
                        props: { type: "text" },
                        on: {
                          click: () => {
                            this.tipShowPlayerEvent(params.row._id, 0);
                          }
                        }
                      },
                      "我的充值"
                    ),
                    h(
                      "Button",
                      {
                        props: { type: "text" },
                        on: {
                          click: () => {
                            this.tipShowPlayerEvent(params.row._id, 1);
                          }
                        }
                      },
                      "我的游戏"
                    )
                  ]
                )
              ]
            );
          }
        },
        {
          title: "登录帐号",
          key: "account",
          ellipsis: true
        },
        {
          title: "玩家ID",
          key: "accountId",
          ellipsis: true
        },
        {
          title: "VIP等级",
          key: "vip",
          ellipsis: true
        },
        {
          title: "金币数量",
          key: "gold",
          ellipsis: true,
          render: (h, params) => {
            return FormatMoney(params.row.gold);
          }
        },
        {
          title: "元宝数",
          key: "goldingot",
          ellipsis: true,
          render: (h, params) => {
            return FormatMoney(params.row.goldingot);
          }
        },
        {
          title: "累计充值(分)",
          key: "totalMoney",
          ellipsis: true
        },
        {
          title: "修改昵称",
          key: "updatename",
          ellipsis: true,
          render: (h, params) => {
            return params.row.updatename > 0 ? "未修改" : "已修改";
          }
        },
        {
          title: "账户类型",
          key: "playerType",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.playerType) {
              case 1:
                return "正式";
              case 0:
                return "游客";
              default:
                return "机器人";
            }
          }
        },
        {
          title: "玩家状态",
          key: "frozen",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.frozen) {
              case 1:
                return "锁定";
              default:
                return "正常";
            }
          }
        },
        {
          title: "最后登录时间",
          key: "lastLoginTime",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.lastLoginTime, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "注册时间",
          key: "registerTime",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.registerTime, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "操作",
          key: "action",
          width: 200,
          align: "center",
          render: (h, params) => {
            return h("div", [
              h("Button", {
                props: {
                  type: "text",
                  icon: "eye",
                  size: "small"
                },
                style: {
                  marginRight: "2px"
                },
                on: {
                  click: () => {
                    this.$Modal.info({
                      title: "玩家信息",
                      scrollable: true,
                      content: `名称：${this.dataShow[params.index]
                        .name}<br>类型：${this.dataShow[params.index].playerType ==
                      1
                        ? "正式玩家"
                        : "试玩玩家"}<br>VIP等级：${this.dataShow[params.index]
                        .vip}<br>金币数：${this.dataShow[params.index]
                        .gold}<br>元宝数：${this.dataShow[params.index].goldingot}`
                    });
                  }
                }
              }),
              h("Button", {
                props: {
                  type: "text",
                  icon: "arrow-up-c",
                  size: "small"
                },
                style: {
                  marginRight: "2px"
                },
                on: {
                  click: () => {
                    this.goldNumber = 100;
                    this.$Modal.confirm({
                      title: "玩家上分(请填写正数)",
                      render: h => {
                        return h("InputNumber", {
                          props: {
                            min: 0,
                            value: this.goldNumber
                          },
                          on: {
                            input: val => {
                              this.goldNumber = val;
                            }
                          }
                        });
                      },
                      onOk: () => {
                        if (this.goldNumber <= 0) return;
                        this.changePlayerGoldEvent(
                          params.row._id,
                          this.goldNumber
                        );
                      }
                    });
                  }
                }
              }),
              h("Button", {
                props: {
                  type: "text",
                  icon: "arrow-down-c",
                  size: "small"
                },
                style: {
                  marginRight: "2px"
                },
                on: {
                  click: () => {
                    this.goldNumber = -100;
                    this.$Modal.confirm({
                      title: "玩家下分(请填写负数)",
                      render: h => {
                        return h("InputNumber", {
                          props: {
                            max: 0,
                            value: this.goldNumber
                          },
                          on: {
                            input: val => {
                              this.goldNumber = val;
                            }
                          }
                        });
                      },
                      onOk: () => {
                        if (this.goldNumber >= 0) return;
                        this.changePlayerGoldEvent(
                          params.row._id,
                          this.goldNumber
                        );
                      }
                    });
                  }
                }
              }),
              h("Button", {
                props: {
                  type: "text",
                  icon: "locked",
                  size: "small"
                },
                style: {
                  marginRight: "2px"
                },
                on: {
                  click: () => {
                    this.$Modal.confirm({
                      title: "系统提示",
                      content: "确定锁定选择项?",
                      onOk: () => {
                        this.freezePlayerEvent(params.row._id, 1);
                      }
                    });
                  }
                }
              }),
              h("Button", {
                props: {
                  type: "text",
                  icon: "unlocked",
                  size: "small"
                },
                on: {
                  click: () => {
                    this.$Modal.confirm({
                      title: "系统提示",
                      content: "确定解锁选择项?",
                      onOk: () => {
                        this.freezePlayerEvent(params.row._id, 0);
                      }
                    });
                  }
                }
              }),
              h("Button", {
                props: {
                  type: "text",
                  icon: "social-vimeo",
                  size: "small"
                },
                on: {
                  click: () => {
                    this.$Modal.confirm({
                      title: "修改VIP等级",
                      render: h => {
                        return h("InputNumber", {
                          props: {
                            max: 9,
                            min: 0,
                            value: this.playerVIP
                          },
                          on: {
                            input: val => {
                              this.playerVIP = val;
                            }
                          }
                        });
                      },
                      onOk: () => {
                        if (this.playerVIP < 0 || this.playerVIP > 9) return;
                        this.changePlayerVIPEvent(
                          params.row._id,
                          this.playerVIP
                        );
                      }
                    });
                  }
                }
              })
            ]);
          }
        }
      ],
      searchModel: {
        name: "",
        playerType: null,
        frozen: null,
        p: 1,
        method: "all",
        gameId: "",
        modalTimes: [
          this.moment()
            .add(-7, "days")
            .format("YYYY-MM-DD"),
          this.moment().format("YYYY-MM-DD")
        ], //modal 层查询用
        pageTimes: []
      },
      modalType: 0,
      modelDatas: [],
      modalColumns: [],
      modalTotal: 0,
      modalPageSize: 5,
      modalCurrentPage: 1,
      modalPlayerId: ""
    };
  },
  methods: {
    searchEvent: function() {
      this.searchModel.p = 1;
      this.initTableData();
    },
    modalSearchEvent: function() {
      this.initModalTable(this.modalPlayerId, this.modalType);
    },
    confirmOkEvent: function() {
      this.$Notice.success({
        title: "操作成功"
      });
    },
    pickerSearchEvent: function(times) {
      this.searchModel.modalTimes = times;
    },
    pageChange: function(page) {
      this.searchModel.p = page;
      this.currentPage = page;
      this.initTableData();
    },
    modalPageChangeEvant: function(page) {
      this.modalCurrentPage = page;
      this.initModalTable(this.modalPlayerId, this.modalType);
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_PLAYER_LIST, this.searchModel)
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
    changePlayerGoldEvent: function(_playerId, _gold) {
      let data = { playerId: _playerId, gold: _gold };
      HttpPost(
        HTTP_URL_API.PLAYER_ADD_GOLD,
        MakeSign(data),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.$Notice.success({
            title: "提交成功"
          });
        } else
          this.$Notice.error({
            title: "提交失败"
          });
      });
    },
    changePlayerVIPEvent: function(_playerId, _vip) {
      let data = { playerId: _playerId, vip: _vip };
      HttpPost(
        HTTP_URL_API.PLAYER_ADD_GOLD,
        MakeSign(data),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.$Notice.success({
            title: "提交成功"
          });
        } else
          this.$Notice.error({
            title: "提交失败"
          });
      });
    },
    freezePlayerEvent: function(_playerId, _frozen) {
      let data = { playerId: _playerId, frozen: _frozen };
      HttpPost(
        HTTP_URL_API.SET_PLAYER_FREEZE,
        MakeSign(data),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.$Notice.success({
            title: "提交成功"
          });
        } else
          this.$Notice.error({
            title: "提交失败"
          });
      });
    },
    tipShowPlayerEvent: function(_playerId, _modalType) {
      this.modalType = Number(_modalType) || 0;
      this.modalPlayerId = _playerId;
      switch (_modalType) {
        case 0:
          this.modalTitle = "我的充值";
          this.modalColumns = [
            {
              title: "商品编号",
              key: "shopId"
            },
            {
              title: "充值金币",
              key: "total_fee",
              render: (h, params) => {
                return FormatMoney(params.row.total_fee);
              }
            },
            {
              title: "交易流水号",
              key: "transactionId"
            },
            {
              title: "创建时间",
              key: "createtime",
              width: 180,
              render: (h, params) => {
                return this.Moment(
                  params.row.createtime,
                  "YYYY-MM-DD HH:mm:ss"
                );
              }
            },
            {
              title: "交易状态",
              key: "status"
            },
            {
              title: "支付时间",
              key: "paytime",
              width: 180,
              render: (h, params) => {
                return this.Moment(params.row.paytime, "YYYY-MM-DD HH:mm:ss");
              }
            }
          ];
          break;
        case 1:
          this.modalTitle = "我的游戏";
          this.modalColumns = [
            {
              type: "expand",
              width: 50,
              render: (h, params) => {
                return h(VmExpandBets, {
                  props: {
                    row: params.row.bets
                  }
                });
              }
            },
            {
              title: "游戏编号",
              key: "gameId"
            },
            {
              title: "投注金额",
              key: "allBets",
              render: (h, params) => {
                return FormatMoney(params.row.allBets);
              }
            },
            {
              title: "是否结算",
              key: "issettle",
              render: (h, params) => {
                return params.row.issettle == 1 ? "已结算" : "未结算";
              }
            },
            {
              title: "结算金额",
              key: "win",
              render: (h, params) => {
                return FormatMoney(params.row.win);
              }
            },
            {
              title: "是否坐庄",
              key: "isBanker",
              render: (h, params) => {
                return params.row.isBanker == 0 ? "否" : "是";
              }
            },
            {
              title: "开奖结果",
              key: "result",
              render: (h, params) => {
                if (typeof params.row.result == "object")
                  return params.row.result.win.join(",");
                else return params.row.result;
              }
            },
            {
              title: "投注时间",
              key: "startTime",
              width: 180,
              render: (h, params) => {
                return this.Moment(params.row.startTime, "YYYY-MM-DD HH:mm:ss");
              }
            }
          ];
          break;
      }
      this.initModalTable(this.modalPlayerId, this.modalType);
    },
    initModalTable: function(_playerId, _modalType) {
      let stime = this.searchModel.modalTimes[0] || "";
      let etime = this.searchModel.modalTimes[1] || "";
      this.modalShow = true;
      this.modelDatas = [];
      this.modalTotal = 0;
      HttpGet(HTTP_URL_API.GET_PLAYER_RELATED, {
        playerId: _playerId,
        typeId: _modalType,
        p: this.modalCurrentPage,
        size: this.modalPageSize,
        stime: stime,
        etime: etime,
        gameId: this.searchModel.gameId
      }).then(result => {
        if (result && result.data.data.list.length > 0) {
          this.modelDatas = result.data.data.list;
          this.modalTotal = result.data.data.count;
        }
      });
    }
  },
  mounted: function() {
    this.initTableData();
  },
  components: {
    VmExpandBets,
    VmModalTable
  }
};
</script>
