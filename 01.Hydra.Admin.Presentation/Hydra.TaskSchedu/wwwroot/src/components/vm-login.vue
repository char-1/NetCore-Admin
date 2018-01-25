<template>
  <Row class="vm-login vm-panel">
   <Col class="login-form">
     <div class="login-form">
        <Input v-model="username" placeholder="输入登录帐号"></Input>
        <Input v-model="password" type="password" placeholder="输入登录密码"></Input>
        <Button type="primary">登录</Button>
     </div>
   </Col>
  </Row>
</template>
<script>
  export default {
    name: 'VmLogin',
    data: function () {
      return {
        username: '',
        password: '',
        remenber: false
      }
    }
  }
</script>
