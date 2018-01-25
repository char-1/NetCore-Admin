<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
        <div class="edit" v-if="toolbar">
            <Button @click="()=>{modalAdd = true;modalTitle='菜单新增';this.getCascaderPermission();this.eventType='insert';}" ><i class="fa fa-plus"></i> 新增</Button>
            <Button type="warning" @click="editModalClickEvent" ><i class="fa fa-edit"></i> 编辑</Button>
            <Button type="error" @click="deleteModalClickEvent" ><i class="ivu-icon ivu-icon-trash-a"></i> 删除</Button> 
        </div>
        <Tree :data="permissionData" ref="tree"></Tree>
    </div>
    <Modal
        v-model="modalAdd"
        :title="modalTitle"
        :mask-closable="false"
        ok-text="确定"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="addOkEvent('formValidate')"
        @on-cancel="()=>{ this.formValidate.RoutePath=[];this.formValidate.ParentId='';this.formValidate.Name='';this.formValidate.Router='';this.formValidate.Icon='';}">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <Row type="flex">
                <Col span="24">
                    <FormItem label="权限名称" prop="Name">
                        <Input v-model="formValidate.Name" placeholder="请输入权限名称" style="width:325px"></Input>
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
              <FormItem label="父节点" prop="RoutePath">
                <Cascader :data="menuSelectData" v-model="formValidate.RoutePath" change-on-select @on-change="cascaderChangeEvent"></Cascader>
              </FormItem>
              <FormItem label="权限路径" prop="Router">
                <Input v-model="formValidate.Router" placeholder="请输入权限路径" style="width:325px"></Input>
              </FormItem>
              <FormItem label="路径标识" prop="RouterName">
                <Input v-model="formValidate.RouterName" placeholder="请输入路径标识" style="width:325px"></Input>
              </FormItem>              
              <FormItem label="显示排序" prop="SortNumber">
                <InputNumber :max="100000000" :min="0" v-model="formValidate.SortNumber" placeholder="请输入显示排序"></InputNumber>
              </FormItem>
              <FormItem label="是否开启" prop="IsEnable">
                <i-switch size="large" v-model="formValidate.IsEnable">
                  <span slot="open">
                    开启
                  </span>
                  <span slot="close">
                    关闭
                  </span>
                </i-switch>                
              </FormItem>
              <FormItem label="类型" prop="MenuType">
                <Select v-model="formValidate.MenuType" style="width:80px">
                  <Option v-for="item in menuType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                </Select>
              </FormItem>  
              <FormItem label="Icon图标" prop="Icon">
                <Input v-model="formValidate.Icon" placeholder="请输入Icon图标" style="width:325px"></Input>
              </FormItem>                   
            </Row>                                                                                
        </Form>
    </Modal>    
</Row>    
</template>
<script>
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm } from "../../data/utils";
export default {
  data() {
    return {
      title: "系统权限",
      permissionData: [],
      toolbar: true,
      modalAdd: false,
      modalTitle: "菜单新增",
      eventType: "insert",
      formValidate: {
        Id: "",
        Name: "",
        Router: "",
        RouterName: "",
        SortNumber: 0,
        IsEnable: true,
        ParentId: "",
        RoutePath: [],
        MenuType: 0,
        Icon: ""
      },
      ruleValidate: {
        Name: [{ required: true, message: "名称不能为空", trigger: "blur" }],
        Router: [
          { required: true, message: "路径不能为空(空请用 / 代替)", trigger: "blur" }
        ]
        // RoutePath: [
        //   { required: true, type: "array", message: "父节点不能为空", trigger: "blur" }
        // ]
      },
      modaloading: true,
      menuSelectData: [], //菜单级联
      menuType: [{ key: "菜单", value: 0 }, { key: "按钮", value: 1 }] //菜单类型
    };
  },
  mounted: function() {
    this.getIviewPermission();
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
        this.saveMenuClickEvent();
      });
    },
    editModalClickEvent: function() {
      let data = this.$refs.tree.getSelectedNodes();
      if (this._.isEmpty(data)) {
        this.$Message.error("请选择待编辑项");
        return;
      }
      this.getCascaderPermission();
      this.modalAdd = true;
      this.modalTitle = "菜单编辑";
      this.eventType = "update";
      HttpGet(HTTP_URL_API.GET_MENU_SINGLE, {
        menuId: data[0].id
      }).then(result => {
        if (result && !this._.isEmpty(result.data.data)) {
          let reponse = result.data.data;
          this.formValidate.Id = reponse.id;
          this.formValidate.Name = reponse.name;
          this.formValidate.Icon = reponse.icon || "";
          this.formValidate.SortNumber = reponse.sortNumber;
          this.formValidate.ParentId = reponse.parentId;
          this.formValidate.MenuType = reponse.menuType;
          this.formValidate.IsEnable = reponse.isEnable;
          this.formValidate.Router = reponse.router;
          this.formValidate.RouterName = reponse.routerName;
          this.formValidate.RoutePath = reponse.routePath
            ? reponse.routePath.split(",")
            : [];
        }
      });
    },
    deleteModalClickEvent: function() {
      let data = this.$refs.tree.getSelectedNodes();
      if (this._.isEmpty(data)) {
        this.$Message.error("请选择待删除项");
        return;
      }
      this.$Modal.confirm({
        title: "系统提示",
        content: "确定删除选择项?",
        onOk: () => {
          HttpPost(
            HTTP_URL_API.DELETE_MENU_INFO,
            SerializeForm({ menuId: data[0].id })
          ).then(result => {
            if (result && result.data.state == "success") {
              this.$Message.success("提交成功");
              setTimeout(() => {
                this.getIviewPermission();
              }, 500);
            } else this.$Message.warning(result.data.message);
          });
        }
      });
    },
    saveMenuClickEvent: function() {
      HttpPost(
        this.eventType == "insert"
          ? HTTP_URL_API.ADD_MENU_INFO
          : HTTP_URL_API.UPDATE_MENU_INFO,
        SerializeForm(this.formValidate, false)
      ).then(result => {
        if (result && result.data.state == "success" && result.data.data) {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Message.success("提交成功");
          setTimeout(() => {
            this.getIviewPermission();
          }, 500);
        } else {
          this.$Message.error("提交失败");
          this.modalLoadingEvent();
          this.modalAdd = false;
        }
        this.formValidate.RoutePath = [];
        this.formValidate.ParentId = "";
        this.formValidate.Name = "";
        this.formValidate.Router = "";
        this.formValidate.Icon = "";
      });
    },
    getIviewPermission: function() {
      HttpGet(HTTP_URL_API.GET_MENU_TREE, {})
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
    getCascaderPermission: function() {
      HttpGet(HTTP_URL_API.GET_MENU_CASCADER, {}).then(result => {
        if (result && result.data.data.length > 0) {
          this.menuSelectData = result.data.data;
        }
      });
    },
    cascaderChangeEvent: function(value, selectDat) {
      if (value) {
        let _val = this._.last(value);
        this.formValidate.RoutePath=value;
        this.formValidate.ParentId = _val;
      }
    }
  }
};
</script>
