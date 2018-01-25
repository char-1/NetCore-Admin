<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
        <Tabs :animated="false">
            <TabPane label="热更新">
                <Form :model="formItem" :label-width="120">
                    <FormItem label="游戏代码zip包">
                        <Upload action="http://192.168.0.180:17120/API/System/Upload" 
                        :data="uploadData" 
                        :accept="acceptExt"
                        :max-size="maxSize"
                        :format="formatFile"
                        :on-success="successEvent"
                        :on-format-error="formatErrorEvent"
                        :on-exceeded-size="exceededSizeEvent">
                        <Button type="ghost" icon="ios-cloud-upload-outline">选择文件上传</Button>
                        </Upload>
                    </FormItem>
                    <FormItem label="游戏资源存放路径" prop="Update_url">
                        <Input v-model="formItem.Update_url" placeholder="输入游戏资源文件路径"></Input>
                    </FormItem>
                    <FormItem label="版本号" prop="Version">
                        <Input v-model="formItem.Version" placeholder="输入游戏版本号"></Input>
                    </FormItem>                                        
                </Form>
            </TabPane>
            <TabPane label="标签二">标签二的内容</TabPane>
            <TabPane label="标签三">标签三的内容</TabPane>
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
import { HttpGet,HttpPost,SerializeForm } from "../../data/utils";
export default {
  data() {
    return {
      title: "游戏配置",
      showBtnLoading: false,
      formItem: {
        Version: "",
        Code_url: "",
        Update_url: ""
      },
      uploadData: {
        FilePath: "app"
      },
      acceptExt: ".zip",
      maxSize: 1024 * 50,
      formatFile: ["zip"]
    };
  },
  methods: {
    saveEvent: function() {
      if (this.formItem) {
        this.showBtnLoading = true;
        HttpPost(HTTP_URL_API.SET_SYSTEM_CONFIG, SerializeForm(this.formItem))
          .then(result => {
            if (result && result.data.data && result.data.state == "success") {
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
    },
    initConfigEvent: function() {
      HttpGet(HTTP_URL_API.GET_SYSTEM_CONFIG, {file:false}).then(result => {
        if (result && result.data.data) {
          this.formItem.Code_url = result.data.data.code_url;
          this.formItem.Update_url = result.data.data.update_url;
          this.formItem.Version = result.data.data.version;
        }
      });
    },
    successEvent: function(response, file, fileList) {
      let path = this.staticFileUrl+"/app";
      this.formItem.Code_url = path + "/"+response.data.dateDir+"/" + response.data.fileName;
      this.formItem.Update_url = path+ "/"+response.data.dateDir;
    },
    formatErrorEvent: function() {
      this.$Notice.warning({
        title: "格式不允许上传(只允许" + this.acceptExt + ")"
      });
    },
    exceededSizeEvent: function() {
      this.$Notice.warning({
        title: "上传大小超过最大限制(" + this.maxSize / 1024 + "M)"
      });
    }
  },
  mounted: function() {
    this.initConfigEvent();
  }
};
</script>