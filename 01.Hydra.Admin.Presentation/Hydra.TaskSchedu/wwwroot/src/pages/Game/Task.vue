<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <!-- <div class="search-bar">
          <Input placeholder="Please enter ..." v-model="keyword" style="width: 300px"></Input>
          <Button type="ghost" @click="searchEvent"><i class="fa fa-search"></i></Button>
        </div> -->
      </Row>
      <div class="edit" v-if="toolbar">
          <Button @click="modalAdd = true;modalTitle='任务新增';" ><i class="fa fa-plus"></i> 新增</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
    <Modal
        v-model="modalAdd"
        :title="modalTitle"
        :mask-closable="false"
        ok-text="确定"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="addOkEvent('formValidate')">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
       
                    <FormItem label="任务标题" prop="title">
                        <Input v-model="formValidate.title" placeholder="请输入任务标题" />
                    </FormItem>
                
                    <FormItem label="任务描述" prop="describe">
                        <Input v-model="formValidate.describe" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入描述" />
                    </FormItem>
                                            
                    <FormItem label="触发类型" prop="get_type">
                        <Select v-model="formValidate.get_type">
                            <Option v-for="item in triggerType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                        </Select>
                    </FormItem>
                    <FormItem label="奖励类型" prop="type">
                        <Select v-model="formValidate.type">
                            <Option v-for="item in rewardType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                        </Select>
                    </FormItem>
              <Row type="flex">
                        <Col span="12">               
                            <FormItem label="次数触发" prop="max">
                                <InputNumber :max="100000000" :min="1" v-model="formValidate.max" placeholder="请输入次数触发"></InputNumber>
                            </FormItem>
                        </Col>                    
                        <Col span="12">
                            <FormItem label="奖励数量" prop="num">
                                <InputNumber :max="100000000" :min="0" :step="1.0" v-model="formValidate.num" placeholder="请输入奖励数量"></InputNumber>
                            </FormItem>
                        </Col>
              </Row>
              <Row type="flex">
                        <Col span="12">               
                            <FormItem label="排序编号" prop="sortNumber">
                                <InputNumber :max="100000000" :min="0" v-model="formValidate.sortNumber" placeholder="请输入排序编号"></InputNumber>
                            </FormItem>
                            </Col>                    
                        <Col span="12">
                             <FormItem label="是否启用" prop="isenable">
                                <i-switch size="large" v-model="formValidate.isenable">
                                  <span slot="open">
                                    启用
                                  </span>
                                  <span slot="close">
                                    关闭
                                  </span>
                                </i-switch>
                            </FormItem>
                        </Col>
            </Row>                    
            <Row type="flex">
                        <Col span="12">               
                            <FormItem label="任务状态" prop="status">
                                <Select v-model="formValidate.status">
                                    <Option v-for="item in statusType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                                </Select>
                            </FormItem>
                        </Col>
                        <Col span="12">
                            <FormItem label="任务进度" prop="progress">
                                <InputNumber :max="100000000" :min="0" v-model="formValidate.progress" placeholder="请输入任务进度"
                                @on-change="progressEvent"
                                ></InputNumber>
                            </FormItem>
                        </Col>                                            
            </Row>                    
        </Form>
    </Modal>
    <Modal
        v-model="modalDelete"
        title="删除"
        ok-text="确定"
        cancel-text="取消"
        @on-ok="deletOkEvent">
        确定删除选中数据?
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
      title: "任务配置",
      modalTitle: "任务新增",
      keyword: "",
      toolbar: true,
      modalAdd: false,
      modalDelete: false,
      tableLoading: true,
      modalConfirmText: "确定采纳选中数据?",
      modaloading: true,
      total: 0,
      currentPage: 1,
      showNum: 20,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      triggerType: [
        { key: "见面礼", value: 0 },
        { key: "胜利任务", value: 1 },
        { key: "玩游戏", value: 2 },
        { key: "游戏坐庄", value: 3 },
        { key: "充值任务", value: 4 },
        { key: "坐庄爆庄", value: 5 }
      ],
      rewardType: [
        { key: "元宝", value: 1 },
        { key: "金币", value: 2 },
        { key: "点券", value: 3 },
        { key: "比武卡", value: 4 }
      ],
      statusType: [
        { key: "未完成", value: 0 },
        { key: "已完成未领取", value: 1 },
        { key: "已领取", value: 2 }
      ],
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "任务标题",
          key: "title"
        },
        {
          title: "奖励类型",
          key: "type",
          render: (h, params) => {
            switch (params.row.type) {
              case 1:
                return "元宝";
              case 2:
                return "金币";
              case 3:
                return "点券";
              case 4:
                return "比武卡";
            }
          }
        },
        {
          title: "任务描述",
          key: "describe"
        },
        {
          title: "触发类型",
          key: "get_type",
          render: (h, params) => {
            switch (params.row.get_type) {
              case 0:
                return "见面礼";
              case 1:
                return "胜利任务";
              case 2:
                return "玩游戏";
              case 3:
                return "游戏坐庄";
              case 4:
                return "充值任务";
              case 5:
                return "坐庄爆庄";
            }
          }
        },
        {
          title: "次数触发",
          key: "max"
        },
        {
          title: "奖励数量",
          key: "num"
        },
        {
          title: "显示排序",
          key: "sortNumber"
        },
        {
          title: "是否可用",
          key: "isenable",
          render: (h, params) => {
            return h("div", [
              h("i-switch", {
                props: {
                  size: "default",
                  value: params.row.isenable
                },
                on: {
                  "on-change": value => {
                    this.setEnableEvent(value, params.row._id);
                  }
                }
              })
            ]);
          }
        },
        {
          title: "操作",
          key: "action",
          width: 180,
          align: "center",
          render: (h, params) => {
            return h("div", [
              h(
                "Button",
                {
                  props: {
                    type: "info",
                    size: "small"
                  },
                  style: {
                    marginRight: "2px"
                  },
                  on: {
                    click: () => {
                      this.$Modal.info({
                        title: "任务详细",
                        content: `任务名称：${this.dataShow[params.index]
                          .title}<br>任务描述：${this.dataShow[params.index]
                          .describe}<br>次数触发：${this.dataShow[params.index]
                          .max}<br>奖励数量：${this.dataShow[params.index].num}`
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
                    marginRight: "2px"
                  },
                  on: {
                    click: () => {
                      this.modalTitle = "任务编辑";
                      this.modalAdd = true;
                      this.initTaskConfig(params.row._id);
                    }
                  }
                },
                "编辑"
              ),
              h(
                "Button",
                {
                  props: {
                    type: "error",
                    size: "small"
                  },
                  style: {
                    marginRight: "2px"
                  },
                  on: {
                    click: () => {
                      this.dataSelect = [];
                      this.modalDelete = true;
                      this.dataSelect.push(
                        new Array(params.index, params.row._id)
                      );
                    }
                  }
                },
                "删除"
              )
            ]);
          }
        }
      ],
      formValidate: {
        _id: "",
        title: "",
        describe: "",
        type: 2,
        get_type: 2,
        max: 1,
        num: 1,
        status: 0,
        progress: 0,
        sortNumber: 0,
        isenable: true
      },
      ruleValidate: {
        title: [{ required: true, message: "任务名称不能为空", trigger: "blur" }],
        describe: [{ required: true, message: "任务描述不能为空", trigger: "blur" }],
        max: [
          {
            type: "number",
            required: true,
            message: "次数触发为空",
            trigger: "blur"
          }
        ],
        num: [
          {
            type: "number",
            required: true,
            message: "奖励数量不能为空",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    searchEvent: function() {},
    modalLoadingEvent: function() {
      this.modaloading = false;
      this.$nextTick(() => {
        this.modaloading = true;
      });
    },
    addOkEvent: function(name) {
      this.$refs[name].validate(valid => {
        if (!valid) return this.modalLoadingEvent();
        this.saveProduct();
      });
    },
    deletOkEvent: function() {
      if (this.dataSelect) {
        let data = { id: this.dataSelect[0][1] };
        HttpPost(
          HTTP_URL_API.REMOVE_TASK_INFO,
          MakeSign(data),
          "application/json;charset=utf-8"
        ).then(result => {
          if (result && result.data.data.msg == "success") {
            this.modalLoadingEvent();
            this.modalDelete = false;
            this.$Message.success("提交成功");
            this.dataShow.splice(this.dataSelect[0][0], 1);
            this.total -= 1;
            this.dataSelect = [];
          }
        });
      }
    },
    pageChange: function(page) {
      this.currentPage = page;
      this.initTableData(page);
    },
    selectChange: function(data) {
      this.dataSelect = data;
    },
    initTableData: function(__page) {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_TASK_LIST, { p: __page })
        .then(result => {
          if (result && result.data.data.list.length > 0) {
            this.dataShow = result.data.data.list;
            this.total = result.data.data.count;
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
    saveProduct: function() {
      if (this.formValidate.progress > this.formValidate.max)
        this.formValidate.progress = this.formValidate.max;
      HttpPost(
        HTTP_URL_API.SET_TASK_INFO,
        MakeSign(this.formValidate),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data.msg == "success") {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Message.success("提交成功");
          setTimeout(() => {
            this.initTableData(1);
          }, 500);
        }
      });
    },
    progressEvent: function(_event) {
      if (_event > this.formValidate.max)
        this.formValidate.progress = this.formValidate.max;
    },
    setEnableEvent: function(_enable, _taskId) {
      var data = {
        isenable: _enable,
        id: _taskId
      };
      let _data = MakeSign(data);
      HttpPost(
        HTTP_URL_API.ENABLE_TASK_INFO,
        _data,
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success")
          this.$Message.success("操作成功");
        else this.$Message.error("操作失败");
      });
    },
    initTaskConfig: function(_id) {
      if (!_id) this.$Message.error("参数错误");
      HttpGet(HTTP_URL_API.SINGLE_TASK_INFO, { id: _id })
        .then(result => {
          if (result && result.data.data.data) {
            this.formValidate = result.data.data.data;
          } else this.$Message.error("任务不存在");
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 800);
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