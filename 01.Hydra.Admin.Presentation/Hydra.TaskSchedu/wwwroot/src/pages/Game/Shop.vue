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
          <Button @click="modalAdd = true" ><i class="fa fa-plus"></i> 新增</Button>
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
        title="新增商品"
        :mask-closable="false"
        ok-text="确定"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="addOkEvent('formValidate')">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <Row type="flex">
                <Col span="24">
                    <FormItem label="商品名称" prop="name">
                        <Input v-model="formValidate.name" placeholder="请输入商品名称" style="width:325px"></Input>
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
                <Col span="12">                             
                    <FormItem label="商品类型" prop="type">
                        <Select v-model="formValidate.type" style="width:80px" @on-change="productTypeEvent" :label-in-value="true">
                            <Option v-for="item in productType" :value="item.value" :key="item.key">{{ item.key }}</Option>
                        </Select>
                    </FormItem>
                </Col>                    
                <Col span="12">
                    <FormItem label="人民币" prop="rmb">
                        <InputNumber :max="100000000" :min="0" :step="1.0" v-model="formValidate.rmb" placeholder="请输入人民币"></InputNumber>分
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
                <Col span="12">                         
                    <FormItem :label="lableName" prop="goldingot">
                        <InputNumber :max="100000000" :min="0" :step="1.0" v-model="formValidate.goldingot" placeholder="请输入数量"></InputNumber>
                    </FormItem>
                </Col>                    
                <Col span="12">                    
                    <FormItem label="数量" prop="num">
                        <InputNumber :max="100000000" :min="0" :step="1.0" v-model="formValidate.num" placeholder="请输入数量"></InputNumber>
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
                <Col span="24">            
                    <FormItem label="商品编码" prop="productId">
                        <Input v-model="formValidate.productId" placeholder="请输入商品编码" style="width:325px"></Input>
                    </FormItem>  
                </Col>
            </Row>             
            <Row type="flex">
                <Col span="24">            
                    <FormItem label="描述" prop="desc">
                        <Input v-model="formValidate.desc" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="请输入描述" style="width:325px"></Input>
                    </FormItem>  
                </Col>
            </Row>                                                                                
        </Form>
    </Modal>
    <Modal
        v-model="modalDelete"
        title="删除"
        ok-text="确定"
        cancel-text="取消"
        @on-ok="deletOkEvent">
        确定删除选中数据?
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
      title: "充值商品",
      keyword: "",
      toolbar: true,
      modalAdd: false,
      modalDelete: false,
      tableLoading: true,
      modalConfirmText: "确定采纳选中数据?",
      modaloading: true,
      total: 0,
      currentPage: 1,
      showNum: 20,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      lableName: "元宝",
      productType: [
        { key: "元宝", value: 1 },
        { key: "月卡", value: 2 },
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
          title: "编号",
          key: "productId"
        },
        {
          title: "类型",
          key: "type",
          render: (h, params) => {
            switch (params.row.type) {
              case 1:
                return "元宝";
              case 2:
                return "月卡";
              case 3:
                return "金币";
            }
          }
        },
        {
          title: "描述",
          key: "desc"
        },
        {
          title: "人民币",
          key: "rmb"
        },
        {
          title: "元宝数",
          key: "goldingot"
        },
        {
          title: "数量",
          key: "num"
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
                        title: "商品信息",
                        content: `名称：${
                          this.dataShow[params.index].name
                        }<br>编码：${
                          this.dataShow[params.index].productId
                        }<br>类型：${
                          this.dataShow[params.index].type == 1
                            ? "元宝"
                            : "月卡"
                        }<br>描述：${
                          this.dataShow[params.index].desc
                        }<br>人民币：${
                          this.dataShow[params.index].rmb
                        }<br>元宝数：${this.dataShow[params.index].goldingot}`
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
                      this.dataSelect = [];
                      this.modalDelete = true;
                      this.dataSelect.push(
                        new Array(params.index, params.row._id)
                      );
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
        name: "",
        type: 1,
        desc: "",
        rmb: 0,
        num: 1,
        goldingot: 0,
        productId: ""
      },
      ruleValidate: {
        name: [{ required: true, message: "名称不能为空", trigger: "blur" }],
        productId: [
          { required: true, message: "编码不能为空", trigger: "blur" }
        ],
        desc: [{ required: true, message: "描述不能为空", trigger: "blur" }],
        rmb: [
          {
            type: "number",
            required: true,
            message: "人民币不能为空",
            trigger: "blur"
          }
        ],
        goldingot: [
          {
            type: "number",
            required: true,
            message: "钻石数不能为空",
            trigger: "blur"
          }
        ]
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
    deletOkEvent: function() {
      if (this.dataSelect) {
        let data = { id: this.dataSelect[0][1] };
        HttpPost(
          HTTP_URL_API.DEL_SHOP_PRODUCT,
          MakeSign(data),
          "application/json;charset=utf-8"
        ).then(result => {
          if (result && result.data.data == "success") {
            this.modalLoadingEvent();
            this.modalDelete = false;
            this.$Message.success("提交成功");
            this.dataShow.splice(this.dataSelect[0][0], 1);
            this.total -= 1;
          } else this.$Message.error("操作失败");
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
      HttpGet(HTTP_URL_API.GET_SHOP_PRODUCT, { p: __page })
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
      delete this.formValidate.sign;
      HttpPost(
        HTTP_URL_API.ADD_SHOP_PRODUCT,
        MakeSign(this.formValidate),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.dataShow.unshift(this.formValidate);
          this.total += 1;
          this.$Message.success("提交成功");
          this.resetForm();
        } else this.$Message.error("操作失败");
      });
    },
    productTypeEvent: function(data) {
      this.lableName = data.label;
    },
    resetForm: function() {
      this.formValidate = {
        name: "",
        type: 1,
        desc: "",
        rmb: 0,
        num: 1,
        goldingot: 0,
        productId: ""
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