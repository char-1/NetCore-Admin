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
          :clearable="false"
          placement="bottom-start" 
          placeholder="开始时间-结束时间" 
          v-model="times" 
          format="yyyy-MM-dd"
          @on-change='getDatepicker'
          style="width: 180px"></DatePicker>
          </Col>          
          <Col>
              <Input placeholder="玩家ID编号" v-model="searchModel.keyword" style="width: 200px"/>
          </Col>
          <Col>
              <Button type="ghost" @click="searchEvent"><i class="fa fa-search">查询</i></Button>
          </Col>
        </Row>
        </div>
      </Row>
      <div class="edit" v-if="toolbar">
          <Button  :disabled="adoptDisabled"><i class="fa fa-plus"></i>删除</Button>
      </div>
      <Table ref="selection" :loading="tableLoading" :stripe="showStripe" :size="tableSize" :columns="showColumns" :data="dataShow"></Table>
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
      title: "抽奖记录",
      toolbar: false,
      tableLoading: true,
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
          title: "名称",
          key: "name",
          ellipsis: true
        },
        {
          title: "数量",
          key: "num",
          ellipsis: true
        },
        {
          title: "类型",
          key: "type",
          ellipsis: true,
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
          title: "抽奖时间",
          key: "time",
          ellipsis: true,
          render: (h, params) => {
            return this.Moment(params.row.time, "YYYY-MM-DD HH:mm:ss");
          }
        }
      ],
      searchModel: {
        keyword: "",
        p: 1,
        stime: this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        etime: this.moment().format("YYYY-MM-DD")
      },
      times: [
        this.moment()
          .add(-7, "days")
          .format("YYYY-MM-DD"),
        this.moment().format("YYYY-MM-DD")
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
          }
        ]
      },
      disabledDate: date => {
        return date.getTime() > Date.now();
      }
    };
  },
  methods: {
    getDatepicker: function(times) {
      this.searchModel.stime = times[0];
      this.searchModel.etime = times[1];
    },
    searchEvent: function() {
      this.searchModel.p = 1;
      this.initTableData();
    },
    pageChange: function(page) {
      this.searchModel.p = page;
      this.currentPage = page;
      this.initTableData();
    },
    initTableData: function() {
      this.tableLoading = true;
      HttpGet(HTTP_URL_API.GET_PLAYER_DIALRECORD, this.searchModel)
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
