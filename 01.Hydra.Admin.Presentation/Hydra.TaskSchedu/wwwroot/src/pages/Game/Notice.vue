<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <div class="search-bar">
          <Input placeholder="Please enter ..." v-model="keyword" style="width: 300px"/>
          <Button type="ghost" @click="searchEvent"><i class="fa fa-search"></i></Button>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button @click="modalAdd = true;modalTitle='公告新增';" ><i class="fa fa-plus"></i> 新增</Button>
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
                    <FormItem label="标题" prop="title">
                        <Input v-model="formValidate.title" placeholder="请输入标题" />
                    </FormItem>
                    <FormItem label="描述" prop="content">
                        <Input v-model="formValidate.content" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入描述" />
                    </FormItem>
                                            
                    <FormItem label="类型" prop="type">
                        <Select v-model="formValidate.type">
                            <Option v-for="item in noticeType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                        </Select>
                    </FormItem>
                    <Row type="flex">
                        <Col span="12">               
                            <FormItem label="排序编号" prop="sort">
                                <InputNumber :max="100000000" :min="0" v-model="formValidate.sort" placeholder="请输入排序编号"></InputNumber>
                            </FormItem>
                            </Col>                    
                        <Col span="12">
                             <FormItem label="是否启用" prop="enabled">
                                <i-switch size="large" v-model="formValidate.enabled">
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
        </Form>
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
      title: "通知公告",
      modalTitle: "公告新增",
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
      noticeType: [{ key: "系统公告", value: 0 }],
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
          title: "类型",
          key: "type",
          render: (h, params) => {
            switch (params.row.type) {
              case 0:
                return "系统公告";
            }
          }
        },
        {
          title: "标题",
          key: "title",
          ellipsis: true
        },
        {
          title: "描述",
          key: "content",
          ellipsis: true
        },
        {
          title: "发布时间",
          key: "times",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.times, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "显示排序",
          key: "sort"
        },
        {
          title: "是否可用",
          key: "enabled",
          render: (h, params) => {
            return h("div", [
              h("i-switch", {
                props: {
                  size: "default",
                  value: params.row.enabled
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
                        title: "公告信息",
                        content: `标题：${this.dataShow[params.index]
                          .title}<br>类型：${this.dataShow[params.index].type == 0
                          ? "系统通知"
                          : ""}<br>描述：${this.dataShow[params.index]
                          .content}<br>发布时间：${this.Moment(
                          this.dataShow[params.index].times,
                          "YYYY-MM-DD HH:mm:ss"
                        )}`
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
                      this.modalTitle = "公告编辑";
                      this.modalAdd = true;
                      this.formValidate = {
                        _id: params.row._id,
                        title: params.row.title,
                        content: params.row.content,
                        type: params.row.type,
                        sort: params.row.sort,
                        enabled: params.row.enabled,
                        isnew: false
                      };
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
                      this.$Modal.confirm({
                        title: "系统提示",
                        content: "确定删除选择项?",
                        onOk: () => {
                          HttpPost(
                            HTTP_URL_API.REM_NOTICE_INFO,
                            MakeSign({
                              id: params.row._id
                            }),
                            "application/json;charset=utf-8"
                          ).then(result => {
                            if (
                              result &&
                              result.data.data == "success"
                            ) {
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
      formValidate: {
        _id: "",
        title: "",
        content: "",
        type: 0,
        sort: 0,
        enabled: true,
        isnew: true
      },
      ruleValidate: {
        title: [{ required: true, message: "公告名称不能为空", trigger: "blur" }],
        content: [{ required: true, message: "公告描述不能为空", trigger: "blur" }]
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
        this.saveNotice();
      });
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
      HttpGet(HTTP_URL_API.GET_NOTICE_LIST, { p: __page })
        .then(result => {
          if (result.status == 200) {
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
    saveNotice: function() {
      HttpPost(
        HTTP_URL_API.SET_NOTICE_INFO,
        MakeSign(this.formValidate),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Message.success("提交成功");
          if (this.formValidate.isnew)
            setTimeout(() => {
              this.dataShow.push(this.formValidate);
              this.total += 1;
              this.formValidate = {
                _id: "",
                title: "",
                content: "",
                type: 0,
                sort: 0,
                enabled: true,
                isnew: true
              };
            }, 500);
        } else {
          this.$Message.error(result.data.result.message);
        }
      });
    },
    progressEvent: function(_event) {
      if (_event > this.formValidate.max)
        this.formValidate.progress = this.formValidate.max;
    },
    setEnableEvent: function(_enable, _noticeId) {
      var data = {
        isenable: _enable,
        id: _noticeId
      };
      let _data = MakeSign(data);
      HttpPost(
        HTTP_URL_API.EAB_NOTICE_INFO,
        _data,
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") this.$Message.success("操作成功");
        else this.$Message.error("操作失败");
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