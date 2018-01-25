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
      <Table :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
    <Modal
        v-model="modalAdd"
        title="新增"
        :mask-closable="false"
        ok-text="确定"
        cancel-text="取消"
        :loading="modaloading"
        @on-ok="addOkEvent('formValidate')">
        <Form :label-width="80" ref="formValidate" :model="formValidate" :rules="ruleValidate">
            <FormItem label="敏感词" prop="word">
                <Input v-model="formValidate.word" placeholder="请输入敏感词"></Input>
            </FormItem>
            <FormItem label="是否有效">
                <i-switch v-model="formValidate.isEnable" size="large">
                    <span slot="open">开启</span>
                    <span slot="close">关闭</span>
                </i-switch>
            </FormItem>            
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
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm } from "../../data/utils";
export default {
  data() {
    return {
      title: "系统敏感字",
      keyword: "",
      toolbar: true,
      modalAdd: false,
      modalDelete: false,
      total: 0,
      currentPage: 1,
      tableLoading: false,
      showNum: 50,
      modaloading: true,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      dataSelect: [],
      showColumns: [
        {
          title: "ID",
          key: "_id"
        },
        {
          title: "敏感字",
          key: "word"
        },
        {
          title: "启用",
          key: "isEnable",
          render: (h, params) => {
            return params.row.isEnable ? "启用" : "禁用";
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
                    type: "error",
                    size: "small"
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
        word: "",
        isEnable: true
      },
      ruleValidate: {
        word: [{ required: true, message: "敏感词不能为空", trigger: "blur" }]
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
        this.insertTactful();
      });
    },
    //删除
    deletOkEvent: function() {
      if (this.dataSelect) {
        let data = { id: this.dataSelect[0][1] };
        HttpPost(HTTP_URL_API.TACTFUL_REMOVE, SerializeForm(data)).then(result => {
          if (result && result.data.msg == "success") {
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
    },
    selectChange: function(data) {
      this.dataSelect = data;
    },
    //表格加载
    initTableData: function(__page) {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.TACTFUL_LIST, { p: __page })
        .then(result => {
          if (result && result.data.list.length > 0) {
            this.dataShow = result.data.list;
            this.total = result.data.count;
          }
        })
        .then(() => {
          setTimeout(() => {
            this.tableLoading = false;
          }, 800);
        });
    },
    //新增
    insertTactful: function() {
      HttpPost(
        HTTP_URL_API.TACTFUL_ADD,
        SerializeForm(this.formValidate, false)
      ).then(result => {
        if (result && result.data.msg == "success") {
          this.modalLoadingEvent();
          this.modalAdd = false;
          this.$Message.success("提交成功");
          setTimeout(() => {
            this.initTableData(1);
          }, 500);
        }
      });
    },
    remove(index) {}
  },
  mounted: function() {
    this.initTableData(1);
  }
};
</script>