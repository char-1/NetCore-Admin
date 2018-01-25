<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
      <Row type="flex" justify="space-between" class="control">
        <div class="search-bar">
        <Row type="flex" :gutter="16">
          <Col>
              <Input placeholder="玩家昵称" v-model="searchModel.name" style="width: 200px"></Input>
          </Col>
          <Col>
              <Select v-model="searchModel.frozen" placeholder="玩家状态">
                  <Option v-for="item in playerFreezeObject" :value="item.value" :key="item.key">
                      {{ item.key }}
                  </Option>
              </Select>
          </Col>
          <Col>
              <Select v-model="searchModel.playerType" placeholder="玩家类型">
                  <Option v-for="item in playerTypeObject" :value="item.value" :key="item.key">
                      {{ item.key }}
                  </Option>
              </Select>
          </Col>          
          <Col>
              <Button type="ghost" @click="searchEvent"><i class="fa fa-search">查询</i></Button>
          </Col>
        </Row>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button  :disabled="adoptDisabled" @click="modalAdopt = true"><i class="fa fa-plus"></i> 采纳</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow" @on-selection-change="selectChange"></Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
    <Modal
        v-model="modalAdopt"
        title="删除"
        ok-text="确定"
        cancel-text="取消"
        @on-ok="confirmOkEvent">
        {{modalConfirmText}}
    </Modal>
  </Row>
</template>
<script>
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import { HttpGet,HttpPost,SerializeForm, FormatMoney,MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "充值玩家",
      keyword: "",
      toolbar: false,
      modalConfirm: false,
      adoptDisabled: true,
      tableLoading: true,
      modalConfirmText: "确定采纳选中数据?",
      playerFreezeObject: [
        { key: "全部", value: -1 },
        { key: "正常", value: 0 },
        { key: "锁定", value: 1 }
      ],
      playerTypeObject: [
        { key: "全部", value: -2 },
        { key: "机器人", value: -1 },
        { key: "游客", value: 0 },
        { key: "正式", value: 1 }
      ],
      modalAdopt: false,
      total: 0,
      showNum: 20,
      currentPage: 1,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      goldNumber: 100, //上下分值
      goldType: 1, //上分(-1,下分)
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "昵称",
          key: "name",
          ellipsis: true
        },
        {
          title: "登录帐号",
          key: "account",
          ellipsis: true
        },
        {
          title: "金币数量",
          key: "gold",
          ellipsis: true
        },
        {
          title: "元宝数",
          key: "goldingot",
          ellipsis: true
        },
        {
          title: "累计充值(分)",
          key: "totalMoney",
          ellipsis: true
        },
        {
          title: "修改昵称",
          key: "updatename",
          ellipsis: true,
          render: (h, params) => {
            return params.row.updatename > 0 ? "未修改" : "已修改";
          }
        },
        {
          title: "账户类型",
          key: "playerType",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.playerType) {
              case 1:
                return "正式";
              case 0:
                return "游客";
              default:
                return "机器人";
            }
          }
        },
        {
          title: "玩家状态",
          key: "frozen",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.frozen) {
              case 1:
                return "锁定";
              default:
                return "正常";
            }
          }
        },
        {
          title: "最后登录时间",
          key: "lastLoginTime",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.lastLoginTime, "YYYY-MM-DD HH:mm:ss");
          }
        },
        {
          title: "注册时间",
          key: "registerTime",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.registerTime, "YYYY-MM-DD HH:mm:ss");
          }
        }
      ],
      searchModel: {
        name: "",
        playerType: null,
        frozen: null,
        p: 1,
        method: "recharge"
      }
    };
  },
  methods: {
    searchEvent: function() {
      this.searchModel.p = 1;
      this.initTableData();
    },
    confirmOkEvent: function() {
      this.$Notice.success({
        title: "操作成功"
      });
    },
    pageChange: function(page) {
      this.searchModel.p = page;
      this.currentPage = page;
      this.initTableData();
    },
    selectChange: function(data) {
      this.dataSelect = data;
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_PLAYER_LIST, this.searchModel)
        .then(result => {
          if (result && result.data.data.list.length > 0) {
            this.dataShow = result.data.data.list;
            this.total = result.data.data.count;
          } else {
            this.dataShow = [];
            this.total = 0;
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
    }
  },
  mounted: function() {
    this.initTableData();
  }
};
</script>