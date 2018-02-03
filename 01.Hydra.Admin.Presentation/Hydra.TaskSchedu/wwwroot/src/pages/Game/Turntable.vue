<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
      </Row>
      <div class="edit" v-if="toolbar">
          <Button  :disabled="adoptDisabled" @click="modalEdit = true"><i class="fa fa-plus"></i> 配置</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow"></Table>
    </div>
    <Modal
        v-model="modalEdit"
        title="大转盘配置"
        :mask-closable="false"
        ok-text="确定"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="editOkEvent('formValidate')">
        <Form :label-width="80">
            <FormItem label="名称" prop="name">
                <Input v-model="formValidate.data.name" placeholder="请输入名称"/>
            </FormItem>
            <FormItem label="数值" prop="num">
                <InputNumber :max="100000000" :min="0" v-model="formValidate.data.num" placeholder="请输入数值"></InputNumber>
            </FormItem>
            <FormItem label="中奖率" prop="rate">
                <InputNumber :max="1000" :min="0" v-model="formValidate.data.rate" placeholder="请输入中奖率"></InputNumber>
            </FormItem>  
            <FormItem label="类型" prop="type">
                <Select v-model="formValidate.data.type" style="width:200px">
                    <Option v-for="item in turnTableType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                </Select>
            </FormItem>                               
        </Form>
    </Modal>  
  </Row>
</template>
<script>
import { HTTP_URL_API } from "../../data/api";
import { HttpGet, HttpPost, SerializeForm, MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "大转盘配置",
      keyword: "",
      toolbar: false,
      modalEdit: false,
      adoptDisabled: true,
      tableLoading: true,
      modalConfirmText: "确定采纳选中数据?",
      modalAdopt: false,
      modaloading: true,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      turnTableType: [
        { key: "点券", value: 1 },
        { key: "红包", value: 2 },
        { key: "金币", value: 3 }
      ],
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "名称",
          key: "name"
        },
        {
          title: "数值",
          key: "num"
        },
        {
          title: "中奖率",
          key: "rate"
        },
        {
          title: "类型",
          key: "type",
          render: (h, params) => {
            switch (params.row.type) {
              case 1:
                return "点券";
              case 2:
                return "红包";
              case 3:
                return "金币";
            }
          }
        },
        {
          title: "操作",
          key: "action",
          width: 150,
          align: "center",
          render: (h, params) => {
            return h("div", [
              h(
                "Button",
                {
                  props: {
                    type: "primary",
                    size: "small"
                  },
                  style: {
                    marginRight: "5px"
                  },
                  on: {
                    click: () => {
                      this.modalEdit = true;
                      this.formValidate.data.id = params.row.id;
                      this.formValidate.data.name = params.row.name;
                      this.formValidate.data.num = params.row.num;
                      this.formValidate.data.type = params.row.type;
                      this.formValidate.data.rate = params.row.rate;
                    }
                  }
                },
                "配置"
              )
            ]);
          }
        }
      ],
      formValidate: {
        _id: "fortune",
        data: {
          id: 0,
          name: "",
          num: 0,
          type: 3,
          rate: 0
        }
      }
    };
  },
  methods: {
    modalLoadingEvent: function() {
      this.modaloading = false;
      this.$nextTick(() => {
        this.modaloading = true;
      });
    },
    editOkEvent: function(name) {
      this.saveDialConfig();
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_SINGLE_CONFIG, { _id: "fortune" })
        .then(result => {
          if (
            result.data.state == "success" &&
            result.data.data.length > 0
          ) {
            this.dataShow = result.data.data;
          } else this.$Message.error("加载出错");
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 800);
        });
    },
    saveDialConfig: function() {
      HttpPost(
        HTTP_URL_API.SET_SINGLE_CONFIG,
        MakeSign(this.formValidate),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data.msg == "success") {
          this.modalLoadingEvent();
          this.modalEdit = false;
          this.dataShow = result.data.data.data;
          this.$Notice.success({
            title: "操作成功"
          });
          this.formValidate = {
            _id: "fortune",
            data: {
              id: 0,
              name: "",
              num: 0,
              type: 3,
              rate: 0
            }
          };
        }
      });
    }
  },
  mounted: function() {
    this.initTableData();
  }
};
</script>