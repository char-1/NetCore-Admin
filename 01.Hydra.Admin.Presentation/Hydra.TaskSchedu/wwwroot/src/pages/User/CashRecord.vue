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
          v-model="times" 
          format="yyyy-MM-dd"
          @on-change='getDatepicker'
          style="width: 180px"></DatePicker>
          </Col>          
          <Col>
              <Input placeholder="玩家昵称/ID编号" v-model="searchModel.name" style="width: 200px"/>
          </Col>
          <Col>
              <Select v-model="searchModel.typeId" placeholder="订单类型" @on-change="orderTypeEvent" style="width:100px">
                  <Option v-for="item in orderTypeSelect" :value="item.value" :key="item.key">
                      {{ item.key }}
                  </Option>
              </Select>
          </Col>
          <Col>
              <Select v-model="searchModel.status" :placeholder="statusPlaceholder" :disabled="disabledStatus" style="width:100px">
                  <Option v-for="item in orderStatusSelect" :value="item.value" :key="item.key">
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
        v-model="modalCash"
        title="提现处理"
        :mask-closable="false"
        ok-text="确定"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="addOkEvent('formValidate')">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <FormItem label="是否打款" prop="status">
                <i-switch size="large" v-model="formValidate.status" :true-value="1" :false-value="9">
                  <span slot="open">
                    已打款
                  </span>
                  <span slot="close">
                    拒绝打款
                  </span>
                </i-switch>
            </FormItem>  
            <FormItem label="处理描述" prop="handleMark">
                <Input v-model="formValidate.handleMark" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入处理描述(如:处理时间/地点/人物)" />
            </FormItem>                             
        </Form>
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
      title: "充值提现",
      keyword: "",
      toolbar: false,
      adoptDisabled: true,
      tableLoading: true,
      modalCash: false,
      disabledStatus: true,
      modaloading: true,
      statusPlaceholder: "-请选择-",
      orderTypeSelect: [
        { key: "全部", value: -1 },
        { key: "充值", value: 0 },
        { key: "提现", value: 1 }
      ],
      orderStatusSelect: [],
      total: 0,
      showNum: 20,
      currentPage: 1,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "昵称",
          key: "nickName",
          ellipsis: true,
          render: (h, params) => {
            if (params.row.typeId == 1 && params.row.status == 0) {
              return h(
                "Poptip",
                {
                  props: {
                    trigger: "hover",
                    title: "操作",
                    placement: "bottom"
                  }
                },
                [
                  h("p", params.row.nickName),
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
                        "操作"
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
                              this.handleCashRecord(
                                params.row._id,
                                params.row.typeId,
                                params.row.status
                              );
                            }
                          }
                        },
                        "提现处理"
                      )
                    ]
                  )
                ]
              );
            } else return params.row.nickName;
          }
        },
        {
          title: "玩家ID",
          key: "accountId",
          ellipsis: true
        },
        {
          title: "类型",
          key: "typeId",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.typeId) {
              case 0:
                return "充值";
              default:
                return "提现";
            }
          }
        },
        {
          title: "金额",
          key: "amount",
          ellipsis: true,
          render: (h, params) => {
            return FormatMoney(params.row.amount, 0);
          }
        },
        {
          title: "操作时间",
          key: "applyTime",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.applyTime, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "状态",
          key: "status",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.status) {
              case 0:
                return "待打款";
              case 1:
                return "已打款";
              default:
                return "已拒绝";
            }
          }
        },
        {
          title: "支付/处理时间",
          key: "handleTime",
          ellipsis: true,
          render: (h, params) => {
            return params.row.handleTime == 0
              ? ""
              : this.Moment(params.row.handleTime, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "备注",
          key: "handleMark",
          ellipsis: true
        },
        {
          title: "操作",
          key: "action",
          align: "center",
          ellipsis: true,
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
                    let cashStr =
                      this.dataShow[params.index].typeId == 1
                        ? `<br>银行卡号：${this.dataShow[params.index].cashAccount.cardNumber}<br>真实姓名：${this.dataShow[params.index].cashAccount.realName}`
                        : ``;
                    this.$Modal.info({
                      title: "数据详情",
                      scrollable: true,
                      content:
                        `玩家昵称：${this.dataShow[params.index]
                          .nickName}<br>玩家ID：${this.dataShow[params.index]
                          .accountId}<br>类型：${this.dataShow[params.index]
                          .typeId == 0
                          ? "充值"
                          : "提现"}<br>金额：${FormatMoney(
                          this.dataShow[params.index].amount,
                          0
                        )}<br>申请时间：${this.Moment(
                          this.dataShow[params.index].applyTime,
                          "YYYY-MM-DD HH:mm:ss"
                        )}` + cashStr
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
        status: -1,
        p: 1,
        typeId: -1,
        stime: this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        etime: this.moment().format("YYYY-MM-DD")
      },
      times: [
        this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        this.moment().format("YYYY-MM-DD")
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
        ]
      },
      disabledDate: date => {
        return date.getTime() > Date.now();
      },
      formValidate: {
        _id: "",
        status: 1,
        handleMark: ""
      },
      ruleValidate: {
        handleMark: [{ required: true, message: "描述不能为空", trigger: "blur" }]
      }
    };
  },
  methods: {
    modalLoadingEvent: function() {
      this.modaloading = false;
      this.$nextTick(() => {
        this.modaloading = true;
      });
    },
    addOkEvent: function(name) {
      this.$refs[name].validate(valid => {
        if (!valid) return this.modalLoadingEvent();
        else {
          delete this.formValidate.sign;
          HttpPost(
            HTTP_URL_API.SET_PLAYER_CASHRECORD,
            MakeSign(this.formValidate),
            "application/json;charset=utf-8"
          ).then(result => {
            if (result && result.data.data == "success") {
              this.modalCash = false;
              this.$Notice.success({
                title: "操作成功"
              });
              this.formValidate = {
                _id: "",
                status: 1,
                handleMark: ""
              };
            } else {
              this.$Notice.error({
                title: "操作失败(已提交待同步状态)"
              });
              return this.modalLoadingEvent();
            }
          });
        }
      });
    },
    getDatepicker: function(times) {
      this.searchModel.stime = times[0];
      this.searchModel.etime = times[1];
    },
    searchEvent: function() {
      this.searchModel.p = 1;
      this.initTableData();
    },
    pageChange: function(page) {
      this.searchModel.p = page;
      this.currentPage = page;
      this.initTableData();
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_PLAYER_ORDER, this.searchModel)
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
    orderTypeEvent: function(data) {
      switch (data) {
        case -1:
          this.orderStatusSelect = [];
          this.disabledStatus = true;
          this.statusPlaceholder = "-请选择-";
          break;
        case 0:
          this.disabledStatus = false;
          this.statusPlaceholder = "充值状态";
          this.orderStatusSelect = [
            { key: "全部", value: -1 },
            { key: "待支付", value: 0 },
            { key: "已支付", value: 1 }
          ];
          break;
        case 1:
          this.disabledStatus = false;
          this.statusPlaceholder = "提现状态";
          this.orderStatusSelect = [
            { key: "全部", value: -1 },
            { key: "待打款", value: 0 },
            { key: "已打款", value: 1 },
            { key: "已拒绝", value: 9 }
          ];
          break;
      }
    },
    handleCashRecord: function(_cashId, _typdId, _status) {
      if (_typdId != 1 || _status != 0)
        this.$Notice.warning({
          title: "不允许操作"
        });
      else {
        this.formValidate._id = _cashId;
        this.modalCash = true;
      }
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
