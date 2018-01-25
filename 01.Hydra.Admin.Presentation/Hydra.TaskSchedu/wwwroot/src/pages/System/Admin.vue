<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <div class="search-bar">
          <Input placeholder="Please enter ..." v-model="keyword" style="width: 300px"></Input>
          <Button type="ghost" @click="searchEvent"><i class="fa fa-search"></i></Button>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button @click="()=>{modalAdd = true;modalTitle='管理员新增';getRoleSelectEvent();eventType='insert';}" ><i class="fa fa-plus"></i> 新增</Button>
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
        @on-ok="addOkEvent('formValidate')"
        @on-cancel="()=>{this.clearFormValidate();}">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
                    <FormItem label="登录帐号" prop="LoginName">
                        <Input v-model="formValidate.LoginName" placeholder="请输入登录帐号"></Input>
                    </FormItem>
                    <FormItem label="管理角色" prop="RoleId">
                        <Select v-model="formValidate.RoleId">
                            <Option v-for="item in roleSelect" :value="item.value" :key="item.key">{{ item.key }}</Option>
                        </Select>
                    </FormItem>
                    <FormItem label="登录密码" prop="PassWord">
                        <Input type="password" v-model="formValidate.PassWord" placeholder="请输入登录密码" :disabled="disabledPass"></Input>
                    </FormItem>
                    <FormItem label="是否启用" prop="IsEnable">
                        <i-switch size="large" v-model="formValidate.IsEnable">
                          <span slot="open">
                            启用
                          </span>
                          <span slot="close">
                            关闭
                          </span>
                        </i-switch>
                    </FormItem>
                    <FormItem label="帐号描述" prop="Remark">
                        <Input v-model="formValidate.Remark" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入帐号描述"></Input>
                    </FormItem>                      
        </Form>
    </Modal>
  </Row>
</template>
<script>
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm } from "../../data/utils";
export default {
  data() {
    return {
      title: "系统用户",
      keyword: "",
      toolbar: true,
      modalAdd: false,
      tableLoading: true,
      modalTitle: "管理员新增",
      modaloading: true,
      eventType: "insert",
      total: 0,
      currentPage: 1,
      showNum: 20,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      roleSelect: [],
      disabledPass: false,
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "登录名",
          key: "loginName"
        },
        {
          title: "创建时间",
          key: "createTime"
        },
        {
          title: "最后登录",
          key: "lastLoginTime"
        },
        {
          title: "最后登录IP",
          key: "lastLoginIp"
        },
        {
          title: "最后修改",
          key: "modifyTime"
        },
        {
          title: "描述",
          key: "remark",
          ellipsis: true
        },
        {
          title: "是否可用",
          key: "isEnable",
          render: (h, params) => {
            return h("div", [
              h("i-switch", {
                props: {
                  size: "default",
                  value: params.row.isEnable
                },
                on: {
                  "on-change": value => {
                    this.setEnableEvent(value, params.row.id);
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
                        title: "管理员信息",
                        content: `是否可用：${this.dataShow[params.index].isEnable ==
                        1
                          ? "可用"
                          : "不可用"}<br>描述：${this.dataShow[params.index]
                          .remark}<br>登录帐号：${this.dataShow[params.index]
                          .loginName}<br>最后登录时间：${this.dataShow[params.index]
                          .lastLoginTime}<br>最后登录IP：${this.dataShow[
                          params.index
                        ].lastLoginIp}`
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
                      this.renderClickEvent(params.row.id);
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
                            HTTP_URL_API.DELETE_ADMIN_INFO,
                            SerializeForm({ adminId: params.row.id })
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
                "删除"
              )
            ]);
          }
        }
      ],
      formValidate: {
        LoginName: "",
        RoleId: "",
        PassWord: "",
        IsEnable: true,
        Remark: ""
      },
      ruleValidate: {
        LoginName: [{ required: true, message: "登录名称不能为空", trigger: "blur" }],
        RoleId: [{ required: true, message: "角色不能为空", trigger: "blur" }],
        PassWord: [
          {
            required: true,
            min: 6,
            max: 50,
            message: "登录密码格式错误(不为空且长度6-50位)",
            trigger: "blur"
          }
        ],
        Remark: [{ required: true, message: "管理员描述不能为空", trigger: "blur" }]
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
        this.saveClickEvent();
      });
    },
    renderClickEvent: function(_adminId) {
      if (!_adminId) return;
      this.modalAdd = true;
      this.modalTitle = "管理员编辑";
      this.eventType = "update";
      this.disabledPass = true;
      this.getRoleSelectEvent();
      HttpGet(HTTP_URL_API.GET_ADMIN_SINGLE, {
        adminId: _adminId
      }).then(result => {
        if (result && !this._.isEmpty(result.data.data)) {
          let reponse = result.data.data;
          this.formValidate.Id = reponse.id;
          this.formValidate.LoginName = reponse.loginName;
          this.formValidate.RoleId = reponse.roleId;
          this.formValidate.IsEnable = reponse.isEnable;
          this.formValidate.Remark = reponse.remark;
          this.formValidate.PassWord = reponse.passWord;
        }
      });
    },
    deletOkEvent: function() {
      if (this.dataSelect) {
        let data = { adminId: this.dataSelect[0][1] };
        HttpPost(
          HTTP_URL_API.DELETE_ADMIN_INFO,
          SerializeForm(data)
        ).then(result => {
          if (result && result.data.state == "success" && result.data.data) {
            this.modalLoadingEvent();
            this.modalDelete = false;
            this.$Message.success("提交成功");
            this.dataShow.splice(this.dataSelect[0][0], 1);
            this.total -= 1;
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
      HttpGet(HTTP_URL_API.GET_ADMIN_LIST, { p: __page })
        .then(result => {
          if (result && result.data.state =="success") {
            this.dataShow = result.data.data.rows;
            this.total = result.data.data.total;
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
    saveClickEvent: function() {
      this.formValidate.PassWord = this.md5(this.formValidate.PassWord);
      HttpPost(
        this.eventType == "insert"
          ? HTTP_URL_API.ADD_ADMIN_INFO
          : HTTP_URL_API.UPDATE_ADMIN_INFO,
        SerializeForm(this.formValidate, false)
      ).then(result => {
        if (result && result.data.state == "success" && result.data.data) {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Message.success("提交成功");
          setTimeout(() => {
            this.initTableData(1);
            this.clearFormValidate();
          }, 500);
        }
      });
    },
    setEnableEvent: function(_enable, _adminId) {
      var data = {
        IsEnable: _enable,
        Id: _adminId
      };
      let _data = SerializeForm(data);
      HttpPost(HTTP_URL_API.SET_ADMIN_ENABLE, _data).then(result => {
        if (result && result.data.state == "success" && result.data.data) {
          this.$Message.success("操作成功");
        } else this.$Message.error("操作失败");
      });
    },
    getRoleSelectEvent: function() {
      HttpGet(HTTP_URL_API.GET_ROLE_SELECT, {})
        .then(result => {
          if (result && !this._.isEmpty(result.data.data)) {
            this.roleSelect = result.data.data;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 800);
        });
    },
    clearFormValidate: function() {
      this.formValidate = {
        LoginName: "",
        RoleId: "",
        PassWord: "",
        IsEnable: true,
        Remark: ""
      };
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