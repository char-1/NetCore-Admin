<template>
  <div class="login">
    <Row class="vm-login vm-panel">
      <Col class="login-form">
        <div class="login-form">
            <Form ref="formValidate" :model="formValidate" :rules="ruleValidate">
              <FormItem prop="LoginName">
                <Input v-model="formValidate.LoginName" placeholder="输入登录帐号" icon="person">
                </Input>
              </FormItem>
              <FormItem prop="PassWord">
                <Input v-model="formValidate.PassWord" type="password" placeholder="输入登录密码" icon="locked">
                </Input>
              </FormItem>                
              <Button type="primary" @click="loginEvent('formValidate')">登录</Button>
            </Form>
        </div>
      </Col>
    </Row>
  </div>
</template>
<script>
import { HTTP_URL_API } from "../data/api";
import {HttpGet,HttpPost,SerializeForm} from "../data/utils";
import { mapMutations } from "vuex";
export default {
  data: function() {
    return {
      formValidate: {
        LoginName: "",
        PassWord: ""
      },
      ruleValidate: {
        LoginName: [{ required: true, message: "登录帐号不能为空", trigger: "blur" }],
        PassWord: [{ required: true, message: "登录密码不能为空", trigger: "blur" }]
      }
    };
  },
  methods: {
    ...mapMutations(["ADMIN_STORAGE"]),
    loginEvent: function(name) {
      this.$refs[name].validate(valid => {
        if (valid) {
          let data = {
            LoginName: this.formValidate.LoginName,
            PassWord: this.md5(this.formValidate.PassWord)
          };
          HttpPost(HTTP_URL_API.ADMIN_LOGIN, SerializeForm(data)).then(result => {
            if (result && result.data.state == "success") {
              this.ADMIN_STORAGE(result.data.data);
              this.$Spin.show();
              setTimeout(() => {
                this.$Spin.hide();
                this.$router.push({
                  name: "Dashboard"
                });
              }, 800);
            } else
              this.$Notice.error({
                title: result.data.message
              });
          });
        }
      });
    }
  }
};
</script>
