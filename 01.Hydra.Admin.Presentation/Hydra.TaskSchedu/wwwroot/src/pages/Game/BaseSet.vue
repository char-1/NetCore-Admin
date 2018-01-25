<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
        <Tabs :animated="false">
            <TabPane label="基本信息">
                <Form :model="formItem" :label-width="120">
                    <FormItem label="客服QQ" prop="serviceQQ">
                        <Input v-model="formItem.data.serviceQQ" placeholder="输入客服QQ"/>
                    </FormItem>
                    <FormItem label="客服Wechat" prop="serviceWechat">
                        <Input v-model="formItem.data.serviceWechat" placeholder="输入客服Wechat"/>
                    </FormItem>                    
                    <FormItem label="版权信息" prop="copyRight">
                        <Input v-model="formItem.data.copyRight" placeholder="输入版权信息"/>
                    </FormItem>       
                    <FormItem label="公司名称" prop="companyName">
                        <Input v-model="formItem.data.companyName" placeholder="输入公司名称"/>
                    </FormItem> 
                    <FormItem label="备案信息" prop="recordInfo">
                        <Input v-model="formItem.data.recordInfo" placeholder="输入备案信息"/>
                    </FormItem>     
                    <FormItem label="兑换比例" prop="goldrate">
                        <InputNumber :max="100000000000" :min="1" placeholder="金币兑换比例" v-model="formItem.data.goldrate"></InputNumber>
                    </FormItem>                        
                </Form>
            </TabPane>
            <TabPane label="会员注册">
                <Form :model="formItem" :label-width="120">              
                    <FormItem label="注册协议" prop="registerAnt">
                        <Input v-model="formItem.data.registerAnt" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入注册协议"/>
                    </FormItem>                                                                                         
                </Form>              
            </TabPane>
            <TabPane label="充值/提现">
                <Form :model="formItem" :label-width="120">              
                    <FormItem label="充值">
                      <i-switch size="large" v-model="formItem.data.recharge" >
                        <span slot="open">
                          开启
                        </span>
                        <span slot="close">
                          关闭
                        </span>
                      </i-switch>                         
                    </FormItem>
                    <FormItem label="提现">
                      <i-switch size="large" v-model="formItem.data.exchange" >
                        <span slot="open">
                          开启
                        </span>
                        <span slot="close">
                          关闭
                        </span>
                      </i-switch>                         
                    </FormItem>                                                                                                           
                </Form>              
            </TabPane>            
            <TabPane label="创建机器人">
                <Form :model="formItem" :label-width="120">              
                    <FormItem label="创建机器人">
                      <i-switch size="large" @on-change="switchChangeEvent">
                        <span slot="open">
                          开启
                        </span>
                        <span slot="close">
                          关闭
                        </span>
                      </i-switch>                         
                    </FormItem>
                    <FormItem label="创建机器人数量">
                        <InputNumber :max="100000000000" :min="1" v-model="robotItem.nums">
                            </InputNumber>
                    </FormItem>                                                                                                           
                </Form>              
            </TabPane>
        </Tabs>
        <Row type="flex" justify="end" class="code-row-bg">
          <Button type="primary" @click="saveEvent()" :loading="showBtnLoading" icon="checkmark">
            保存
          </Button>
        </Row>        
    </div>
</Row>
</template>
<script>
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm, MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "系统配置",
      showBtnLoading: false,
      formItem: {
        _id: "baseset",
        data: {
          serviceQQ: "1234567890",
          serviceWechat: "1234567890",
          copyRight: "Copyright 2011-2017",
          companyName: "浙江素宏网络科技有限公司",
          recordInfo: "文网游备字[2017]M-CBG007号",
          registerAnt: "",
          recharge: true,
          exchange: true,
          goldrate: 1
        }
      },
      robotItem: {
        nums: 1,
        min: 10000000,
        max: 100000000
      }
    };
  },
  methods: {
    saveEvent: function() {
      if (this.formItem) {
        this.showBtnLoading = true;
        HttpPost(
          HTTP_URL_API.SET_SINGLE_CONFIG,
          MakeSign(this.formItem),
          "application/json;charset=utf-8"
        )
          .then(result => {
            if (result && result.data.data.msg == "success") {
              this.$Notice.success({
                title: "操作成功"
              });
            } else
              this.$Notice.error({
                title: "操作失败"
              });
          })
          .then(() => {
            setTimeout(() => {
              this.showBtnLoading = false;
            }, 800);
          });
      }
    },
    initConfigEvent: function() {
      HttpGet(HTTP_URL_API.GET_SINGLE_CONFIG, {
        _id: "baseset"
      }).then(result => {
        if (result && result.data.data) {
          this.formItem.data.serviceQQ = result.data.data.serviceQQ;
          this.formItem.data.serviceWechat = result.data.data.serviceWechat;
          this.formItem.data.copyRight = result.data.data.copyRight;
          this.formItem.data.companyName = result.data.data.companyName;
          this.formItem.data.recordInfo = result.data.data.recordInfo;
          this.formItem.data.registerAnt = result.data.data.registerAnt;
          this.formItem.data.recharge = result.data.data.recharge;
          this.formItem.data.exchange = result.data.data.exchange;
          this.formItem.data.goldrate = result.data.data.goldrate;
        }
      });
    },
    switchChangeEvent: function(flag) {
      if (flag && this.robotItem.nums && this.robotItem.nums > 0) {
        HttpPost(
          HTTP_URL_API.ADD_ROBOT_INFO,
          MakeSign(this.robotItem),
          "application/json;charset=utf-8"
        )
          .then(result => {
            if (result && result.data.msg == "success") {
              this.$Notice.success({
                title: "操作成功"
              });
            }
          })
          .then(() => {
            setTimeout(() => {
              this.showBtnLoading = false;
            }, 800);
          });
      }
    }
  },
  mounted: function() {
    this.initConfigEvent();
  }
};
</script>