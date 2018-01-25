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
          <Button @click="modalAdd = true" ><i class="fa fa-plus"></i> 发送</Button>
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
        title="邮件发送"
        :mask-closable="false"
        ok-text="发送"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="addOkEvent('formValidate')"
        @on-cancel="()=>{this.formValidate.playerId='';this.dataAutoComplete=[];this.formValidate.options=[];this.optionsIndex=0;}">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <FormItem label="邮件标题" prop="title">
                <Input v-model="formValidate.title" type="textarea" :autosize="{minRows: 2,maxRows: 5}"
                placeholder="请输入描述" >
                </Input>
            </FormItem>

            <FormItem label="发送对象" prop="sendObject">
                <Select v-model="formValidate.sendObject" @on-change="sendForChangeEvent">
                    <Option v-for="item in sendObjectArray" :value="item.value" :key="item.key">
                        {{ item.key }}
                    </Option>
                </Select>
            </FormItem>
            <FormItem label="发送玩家" prop="playerId" v-show="senf_for_player">
                <AutoComplete
              v-model="formValidate.playerId"
              @on-search="autoCompleteEvent"
              @on-select="(value)=>{this.formValidate.playerId=value;}"
              @on-change="(value)=>{this.formValidate.playerId=value;}"
              placeholder="输入玩家昵称搜索"
              icon="ios-search"
              :clearable="true"
              >
              <Option v-for="option in dataAutoComplete" :value="option._id" :key="option._id">
                  <span class="auto-complete-title">玩家昵称:{{ option.name }}</span>
                  <span class="auto-complete-count">玩家ID:{{ option.accountId }}</span>
              </Option>        
              </AutoComplete>
            </FormItem>
            <FormItem>
              <Button type="dashed" long @click="optionAddEvent" icon="plus-round">添加附件</Button>
            </FormItem>
            <FormItem v-for="(item, index) in formValidate.options" :key="index" :label="'附件 ' + item._id" v-if="item.status">
              <Row>
                <Col span="10">
                  <Select v-model="item.options_type" style="width:120px">
                      <Option v-for="item in optionsType" :value="item.value" :key="item.key">
                          {{ item.key }}
                      </Option>
                  </Select>              
                </Col>
                <Col span="10">
                  <InputNumber :max="999999999999" :min="1" v-model="item.options_number" placeholder="请输入发放附件数量"></InputNumber>              
                </Col>
                <Col span="4"><Button type="dashed" @click="optionRemoveEvent(item._id)" icon="minus-round"></Button></Col>
              </Row>
            </FormItem>

            <FormItem label="邮件来源" prop="sname">
                <Input v-model="formValidate.sname" placeholder="请输入邮件来源" ></Input>
            </FormItem>
            <FormItem label="邮件描述" prop="content">
                <Input v-model="formValidate.content" type="textarea" :autosize="{minRows: 2,maxRows: 5}"
                placeholder="请输入邮件描述" >
                </Input>
            </FormItem>                        
        </Form>
    </Modal>
  </Row>
</template>
<script>
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm, MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "邮件管理",
      keyword: "",
      toolbar: true,
      modalAdd: false,
      modalDelete: false,
      tableLoading: true,
      modalConfirmText: "确定删除选中数据?",
      modaloading: true,
      total: 0,
      currentPage: 1,
      senf_for_player: true,
      showNum: 20,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      dataAutoComplete: [],
      sendObjectArray: [{ key: "指定玩家", value: 0 }, { key: "所有玩家", value: 1 }],
      optionsType: [{ key: "金币", value: 0 }, { key: "元宝", value: 1 }],
      optionsIndex: 0, //邮件附件数量
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "邮件来源",
          key: "sname",
          width: 100
        },
        {
          title: "邮件标题",
          key: "title",
          ellipsis: true
        },
        {
          title: "类型",
          key: "mail_type",
          width: 70,
          render: (h, params) => {
            switch (params.row.mail_type) {
              case 1:
                return "邮件";
              default:
                return "";
            }
          }
        },
        {
          title: "发送对象",
          key: "sendObject",
          width: 100,
          render: (h, params) => {
            switch (params.row.sendObject) {
              case 0:
                return "指定玩家";
              case 1:
                return "所有玩家";
            }
          }
        },
        {
          title: "发送时间",
          key: "times",
          width: 150,
          render: (h, params) => {
            return this.Moment(params.row.times, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "操作",
          key: "action",
          width: 130,
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
                        title: "邮件信息",
                        content: `来源：${this.dataShow[params.index]
                          .sname}<br>类型：${this.dataShow[params.index]
                          .mail_type == 1
                          ? "邮件"
                          : ""}<br>标题：${this.dataShow[params.index]
                          .title}<br>描述：${this.dataShow[params.index].content}`
                      });
                    }
                  }
                },
                "查看"
              )
            ]);
          }
        }
      ],
      formValidate: {
        sname: "赏金娱乐城",
        sendObject: 0,
        title: "",
        content: "",
        playerId: "",
        options: []
      },
      ruleValidate: {
        title: [{ required: true, message: "标题不能为空", trigger: "blur" }],
        playerId: [{ required: true, message: "发送玩家不能为空", trigger: "blur" }]
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
    pageChange: function(page) {
      this.currentPage = page;
      this.initTableData(page);
    },
    selectChange: function(data) {
      this.dataSelect = data;
    },
    initTableData: function(__page) {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_EMAIL_LIST, { p: __page })
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
      this.formValidate.options = this._.filter(
        this.formValidate.options,
        function(item) {
          return item.status;
        }
      );
      HttpPost(
        HTTP_URL_API.SEND_MAIL_INFO,
        MakeSign(this.formValidate),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Notice.success({
            title: "发送成功"
          });
          this.total++;
          setTimeout(() => {
            this.initTableData(1);
            this.formValidate = {
              sname: "赏金娱乐城",
              sendObject: 0,
              title: "",
              content: "",
              playerId: "",
              options: []
            };
          }, 500);
        } else {
          this.$Notice.error({
            title: "发送失败"
          });
          this.modalLoadingEvent();
        }
      });
    },
    autoCompleteEvent: function(_value) {
      if (!_value) return;
      HttpGet(HTTP_URL_API.GET_PLAYER_INFO, {
        account: _value.trim()
      }).then(result => {
        if (result && result.data.data.list.length > 0) {
          this.dataAutoComplete = result.data.data.list;
        }
      });
    },
    sendForChangeEvent: function(_obj) {
      let isVaild = _obj == 0;
      this.ruleValidate.playerId[0].required = isVaild;
      this.senf_for_player = isVaild;
    },
    optionAddEvent: function() {
      if (this.optionsIndex > 1) {
        this.$Message.error("超过最大附件数");
        return;
      }
      this.optionsIndex++;
      this.formValidate.options.push({
        _id: this.optionsIndex,
        options_type: 0,
        options_number: 100,
        status: true
      });
    },
    optionRemoveEvent: function(_index) {
      this.optionsIndex--;
      this._.forEach(this.formValidate.options, item => {
        if (item._id == _index) item.status = false;
      });
    }
  },
  mounted: function() {
    this.initTableData(1);
  }
};
</script>
<<style scoped>
.auto-complete-title{width:300px;padding-right:10px; font-weight: bold;}
.auto-complete-count{padding-left:20px;font-weight: bold;}
</style>
