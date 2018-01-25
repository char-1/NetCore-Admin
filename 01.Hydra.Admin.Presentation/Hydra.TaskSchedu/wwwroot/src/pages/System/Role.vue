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
          <Button @click="()=>{modalAdd = true;modalTitle='角色新增';eventType='insert';}" ><i class="fa fa-plus"></i> 新增</Button>
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
        @on-cancel="()=>{this.formValidate.Name='';this.formValidate.Remark='';}">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <Row type="flex">
                <Col span="24">
                    <FormItem label="角色名称" prop="Name">
                        <Input v-model="formValidate.Name" placeholder="请输入角色名称" style="width:325px"></Input>
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
                <Col span="12">                         
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
                </Col>                    
                <Col span="12">                    
                    <FormItem label="显示排序" prop="SortNumber">
                        <InputNumber :max="99999" :min="0" v-model="formValidate.SortNumber" placeholder="请输入显示排序"></InputNumber>
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
                <Col span="24">            
                    <FormItem label="角色描述" prop="Remark">
                        <Input v-model="formValidate.Remark" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入角色描述" style="width:325px"></Input>
                    </FormItem>  
                </Col>
            </Row>                                                                                
        </Form>
    </Modal>
    <Modal v-model="modalPermission" title="权限分配" ok-text="确定"
        cancel-text="取消"
        :mask-closable="false"
        @on-ok="permissionOkEvent"
        @on-cancel="()=>{this.dataSelect=[];}"
        >
        <Tabs type="card" :animated="false">
            <TabPane label="菜单权限">
                  <Tree :data="permissionData" show-checkbox multiple ref="tree" @on-check-change="treeCheckEvent"></Tree>
            </TabPane>
            <TabPane label="操作权限">
            </TabPane>      
        </Tabs>        
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
      title: "系统角色",
      keyword: "",
      toolbar: true,
      modalAdd: false,
      modalPermission: false,
      tableLoading: true,
      modalConfirmText: "确定采纳选中数据?",
      modalTitle: "角色新增",
      modaloading: true,
      eventType: "insert",
      total: 0,
      currentPage: 1,
      showNum: 20,
      showStripe: true,
      tableSize: "small",
      selectRoleId: "",
      dataShow: [],
      permissionData: [], //权限集合
      dataSelect: [],
      productType: [{ key: "元宝", value: 1 }, { key: "月卡", value: 2 }],
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "登录名",
          key: "name"
        },
        {
          title: "创建时间",
          key: "createTime"
        },
        {
          title: "描述",
          key: "remark"
        },
        {
          title: "排序",
          key: "sortNumber"
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
          width: 230,
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
                        title: "系统角色信息",
                        content: `是否可用：${this.dataShow[params.index].isEnable ==
                        1
                          ? "可用"
                          : "不可用"}<br>描述：${this.dataShow[params.index]
                          .remark}<br>角色名称：${this.dataShow[params.index]
                          .name}<br>创建时间：${this.dataShow[params.index]
                          .createTime}`
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
                            HTTP_URL_API.DELETE_ROLE_INFO,
                            SerializeForm({ roleId: params.row.id })
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
              ),
              h(
                "Button",
                {
                  props: {
                    type: "primary",
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
                    type: "warning",
                    size: "small"
                  },
                  style: {
                    marginRight: "2px"
                  },
                  on: {
                    click: () => {
                      this.modalPermission = true;
                      this.getRolePermission(params.row.id);
                    }
                  }
                },
                "权限"
              )
            ]);
          }
        }
      ],
      formValidate: {
        Id: "",
        Name: "",
        IsEnable: true,
        Remark: "",
        SortNumber: 0
      },
      ruleValidate: {
        Name: [{ required: true, message: "角色名称不能为空", trigger: "blur" }],
        Remark: [{ required: true, message: "角色描述不能为空", trigger: "blur" }]
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
        this.saveRoleEvent();
      });
    },
    renderClickEvent: function(_roleId) {
      if (!_roleId) return;
      this.modalAdd = true;
      this.modalTitle = "角色编辑";
      this.eventType = "update";
      HttpGet(HTTP_URL_API.GET_ROLE_SINGLE, {
        roleId: _roleId
      }).then(result => {
        if (result && !this._.isEmpty(result.data.data)) {
          let reponse = result.data.data;
          this.formValidate.Id = reponse.id;
          this.formValidate.Name = reponse.name;
          this.formValidate.SortNumber = reponse.sortNumber;
          this.formValidate.IsEnable = reponse.isEnable;
          this.formValidate.Remark = reponse.remark;
        }
      });
    },
    deletOkEvent: function() {
      if (this.dataSelect) {
        let data = { roleId: this.dataSelect[0][1] };
        HttpPost(HTTP_URL_API.DELETE_ROLE_INFO, SerializeForm(data)).then(result => {
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
      HttpGet(HTTP_URL_API.GET_ROLE_LIST, { p: __page })
        .then(result => {
          if (result && result.data.data.rows.length > 0) {
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
    Moment: function(date, format) {
      return moment(date).format(format);
    },
    saveRoleEvent: function() {
      HttpPost(
        this.eventType == "insert"
          ? HTTP_URL_API.ADD_ROLE_INFO
          : HTTP_URL_API.UPDATE_ROLE_INFO,
        SerializeForm(this.formValidate, false)
      ).then(result => {
        if (result && result.data.state == "success" && result.data.data) {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Message.success("提交成功");
          setTimeout(() => {
            this.initTableData(1);
          }, 500);
        } else {
          this.$Message.error("提交失败");
          this.modalLoadingEvent();
          this.modalAdd = false;
        }
      });
    },
    setEnableEvent: function(_enable, _roleId) {
      var data = {
        IsEnable: _enable,
        Id: _roleId
      };
      let _data = SerializeForm(data);
      HttpPost(HTTP_URL_API.SET_ROLE_ENABLE, _data).then(result => {
        if (result && result.data.state == "success" && result.data.data) {
          this.$Message.success("操作成功");
        } else this.$Message.error("操作失败");
      });
    },
    getRolePermission: function(_roleId) {
      this.selectRoleId = _roleId;
      HttpGet(HTTP_URL_API.GET_ROLE_PERMISSION, { roleId: _roleId })
        .then(result => {
          if (result && result.data.data.length > 0) {
            this.permissionData = result.data.data;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 800);
        });
    },
    permissionOkEvent: function() {
      if (this.dataSelect.length == 0) {
        let treeNodes = [];
        let _self = this.$refs.tree.getCheckedNodes();
        if (_self) {
          this._.forEach(_self, function(n, key) {
            if (n.levele != 1) treeNodes.push(n.parentId);
            treeNodes.push(n.id);
          });
          this.dataSelect = this._.uniq(treeNodes);
        }
      }
      if (this.dataSelect && this.selectRoleId) {
        var data = {
          roleId: this.selectRoleId,
          permissionNodes: this.dataSelect
        };
        let _data = SerializeForm(data, false);
        HttpPost(HTTP_URL_API.SET_ROLE_PERMISSION, _data).then(result => {
          if (result && result.data.state == "success" && result.data.data) {
            this.$Message.success("操作成功");
          } else this.$Message.error("操作失败");
        });
      }
    },
    treeCheckEvent: function(_self) {
      let treeNodes = [];
      if (_self) {
        this._.forEach(_self, function(n, key) {
          if (n.levele != 1) treeNodes.push(n.parentId);
          treeNodes.push(n.id);
        });
        this.dataSelect = this._.uniq(treeNodes);
      }
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