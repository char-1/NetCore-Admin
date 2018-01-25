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
                <DatePicker 
                type="daterange" 
                :editable="false"
                :options="searchOptions"
                placement="bottom-start" 
                placeholder="操作时间" 
                v-model="datePickerTimes" 
                format="yyyy-MM-dd"
                @on-change='getDatepicker'
                style="width: 180px"></DatePicker>
          </Col>            
          <Col>
              <Input placeholder="玩家ID" v-model="searchModel.accountId" style="width: 200px"/>
          </Col>
          <Col>
              <Select v-model="searchModel.goleType" placeholder="类型" style="width:120px;">
                  <Option v-for="item in goldTypeSelect" :value="item.value" :key="item.key">
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
          <Button  :disabled="adoptDisabled"><i class="fa fa-plus"></i> 删除</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow">
        <div class="ivu-table-footer-body" slot="footer">
          <span>合计:</span>
          <span class="footer-amount"> {{GoldFixed(tableFoot.gold)}}</span>
        </div>
     </Table>
      <Row type="flex" justify="space-between" class="footer">
        <div class="page">
          <Page :total="total" :current="currentPage" :page-size="showNum" @on-change="pageChange" show-total show-elevator></Page>
        </div>
      </Row>
    </div>
  </Row>
</template>
<script>
import moment from "moment";
import { HTTP_URL_API } from "../../data/api";
import {
  HttpGet,
  HttpPost,
  SerializeForm,
  FormatMoney,
  MakeSign
} from "../../data/utils";
export default {
  data() {
    return {
      title: "金币流水",
      keyword: "",
      toolbar: false,
      tableLoading: true,
      datePickerTimes: [
        this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        this.moment().format("YYYY-MM-DD")
      ],
      goldTypeSelect: [
        { key: "全部", value: -1 },
        { key: "任务获得", value: 0 },
        { key: "玩家充值", value: 1 },
        { key: "玩家提现", value: 2 },
        { key: "系统赠送", value: 3 }
      ],
      searchOptions: {
        shortcuts: [
          {
            text: "最近一周",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              return [start, end];
            }
          },
          {
            text: "最近一月",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              return [start, end];
            }
          },
          {
            text: "最近三月",
            value() {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              return [start, end];
            }
          }
        ]
      },
      total: 0,
      showNum: 20,
      currentPage: 1,
      showStripe: true,
      tableSize: "small",
      dataShow: [],
      showColumns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "玩家ID",
          key: "accountId",
          ellipsis: true
        },
        {
          title: "金币数",
          key: "gold",
          ellipsis: true,
          render: (h, params) => {
            return FormatMoney(params.row.gold);
          }
        },
        {
          title: "类型",
          key: "goldType",
          ellipsis: true,
          render: (h, params) => {
            switch (params.row.goldType) {
              case 0:
                return "任务获得";
              case 1:
                return "玩家充值";
              case 2:
                return "玩家提现";
              case 3:
                return "系统赠送";
            }
          }
        },
        {
          title: "备注",
          key: "remark",
          ellipsis: true
        },
        {
          title: "操作时间",
          key: "ignoreTime",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.ignoreTime, "YYYY-MM-DD HH:mm:ss");
          }
        }
      ],
      searchModel: {
        p: 1,
        goleType: -1,
        accountId: "",
        stime: this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        etime: this.moment().format("YYYY-MM-DD")
      },
      tableFoot: {
        gold: 0
      }
    };
  },
  methods: {
    searchEvent: function() {
      this.searchModel.p = 1;
      this.initTableData();
    },
    getDatepicker: function(times) {
      this.searchModel.stime = times[0];
      this.searchModel.etime = times[1];
    },
    pageChange: function(page) {
      this.searchModel.p = page;
      this.currentPage = page;
      this.initTableData();
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_PLAYER_GOLD, this.searchModel)
        .then(result => {
          if (result.data && result.data.data.rows.length > 0) {
            this.dataShow = result.data.data.rows;
            this.total = result.data.data.total;
            this.tableFoot=result.data.data.footer;
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
    },
    GoldFixed(gold) {
      return FormatMoney(gold);
    }
  },
  mounted: function() {
    this.initTableData();
  }
};
</script>
<style scoped>
.ivu-table-footer-body {
  padding-left: 18px;
  padding-right: 18px;
}
.footer-amount {
  padding-left: 20px;
}
</style>

