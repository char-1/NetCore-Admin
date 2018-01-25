<template>
  <div class="home">
	<Row class="header" type="flex" align="middle">
		<div class="logo">
			 游戏后台管理中心
		</div>
    <Poptip trigger="hover" class="login-info" placement="bottom-end">
        <Button type="text">
             <Avatar  size="small" icon="person"/>
        </Button>
        <Row slot="title">
          {{adminInfo.loginName}}
        </Row>         
        <Row slot="content" class="popcontent">
          <Row class="buttonEvent">
            <Col span="12"><Button type="ghost" @click='dropClickEvent("editpass")'>修改密码</Button></Col>
            <Col span="12"><Button type="ghost" @click='dropClickEvent("safeout")'>安全退出</Button></Col>
          </Row>
        </Row>
    </Poptip>  
	</Row>
	<div class="sidebar">
		<Menu theme="light" width="100%" class="menu" :active-name="activeName"
		:accordion="true">
			<Menu-item name="Dashboard">
				<router-link to="/">
					<i class="fa fa-dashboard" style="font-weight: bolder;">
					</i>
					<strong style="font-size:12px;">首页</strong>
				</router-link>
			</Menu-item>
			<Submenu name="DataTable" v-for="item in menuSubArray" :key="item.id">
				<template slot="title">
					<i :class="item.icon" style="font-weight: bolder;">
					</i>
				<strong style="font-size:12px;">{{item.title}}</strong>
				</template>
				<Menu-item :name="items.routerName" v-for="items in item.children" :key="items.id">
					<router-link :to="items.router">
						{{items.title}}
					</router-link>
				</Menu-item>
			</Submenu>
		</Menu>
	</div>
	<div class="main-content">
		<router-view>
		</router-view>
	</div>
	<Modal
        v-model="modalEdit"
        title="修改密码"
        ok-text="确定"
        cancel-text="取消"
		width="450"
        :loading="modaloading"
        @on-ok="modalOkClickEvent('formValidate')">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <FormItem label="原密码" prop="OldPass">
				<Input type="password" v-model="formValidate.OldPass" placeholder="请输入原密码"></Input>
            </FormItem>
            <FormItem label="新密码" prop="NewPass">
				<Input type="password" v-model="formValidate.NewPass" placeholder="请输入新密码"></Input>
            </FormItem>
            <FormItem label="确认密码" prop="ConfimPass">
                <Input type="password" v-model="formValidate.ConfimPass" placeholder="请输入确认密码"></Input>
            </FormItem>
        </Form>
    </Modal>
</div>
</div>

</template>
<script>
import VmMsgPush from "@/components/vm-msg-push.vue";
import { HTTP_URL_API } from "../data/api";
import { HttpGet, HttpPost, SerializeForm } from "../data/utils";
import { mapMutations, mapState } from "vuex";
export default {
  name: "home",
  components: {
    VmMsgPush
  },
  mounted: function() {
    this.activeName = this.$route.name;
    this.initPermissionMeun();
  },
  computed: mapState({
    adminInfo: state => state.adminInfo
  }),
  data() {
    return {
      activeName: "Dashboard",
      modalEdit: false,
      modaloading: true,
      formValidate: {
        AdminId: "",
        OldPass: "",
        NewPass: "",
        ConfimPass: ""
      },
      ruleValidate: {
        OldPass: [
          {
            required: true,
            min: 6,
            max: 50,
            message: "原密码格式错误(不为空且长度6-50位)",
            trigger: "blur"
          }
        ],
        NewPass: [
          {
            required: true,
            min: 6,
            max: 50,
            message: "新密码格式错误(不为空且长度6-50位)",
            trigger: "blur"
          }
        ],
        ConfimPass: [
          {
            required: true,
            min: 6,
            max: 50,
            message: "确认密码格式错误(不为空且长度6-50位)",
            trigger: "blur"
          }
        ]
      },
      menuSubArray: []
    };
  },
  methods: {
    ...mapMutations(["ADMIN_LOGOUT"]),
    modalLoadingEvent: function() {
      this.modaloading = false;
      this.$nextTick(() => {
        this.modaloading = true;
      });
    },
    dropClickEvent: function(name) {
      switch (name) {
        case "editpass":
          this.modalEdit = true;
          break;
        case "safeout":
          this.$Modal.confirm({
            title: "系统提示",
            scrollable:true,
            content: "确定退出管理中心?",
            onOk: () => {
              this.logOut();
              this.ADMIN_LOGOUT({});
              this.$router.push({
                name: "Login"
              });
            }
          });
          break;
      }
    },
    modalOkClickEvent: function(name) {
      this.$refs[name].validate(valid => {
        if (!valid) return this.modalLoadingEvent();
      });
      if (this.formValidate.NewPass !== this.formValidate.ConfimPass) {
        this.$Message.error("确认密码输入不正确");
        return this.modalLoadingEvent();
      }
      let data = {
        AdminId: this.adminInfo.id,
        OldPass: this.md5(this.formValidate.OldPass),
        NewPass: this.md5(this.formValidate.AdminId)
      };
      HttpPost(
        HTTP_URL_API.UPDATE_ADMIN_PASS,
        SerializeForm(data, false)
      ).then(result => {
        if (result && result.data.state == "success" && result.data.data) {
          this.$Message.success("操作成功");
        } else {
          this.$Message.error(result.data.message);
          return this.modalLoadingEvent();
        }
      });
    },
    initPermissionMeun: function() {
      HttpGet(HTTP_URL_API.GET_ROLE_PERMISSION, {
        roleId: this.adminInfo.roleId,
        isMenu: true
      }).then(result => {
        if (result && result.data.data.length > 0) {
          this.menuSubArray = result.data.data;
        }
      });
    },
    logOut() {
      let data = {
        token: this.adminInfo.lastLoginToken
      };
      HttpPost(
        HTTP_URL_API.ADMIN_LOGOUT,
        SerializeForm(data, false)
      ).then(result => {});
    }
  }
};
</script>
<<style scoped>
.popcontent{width:150px;height:40px;}
.buttonEvent{margin-top:8px;}
</style>

