<template>
<div>
  <div class="modal-search-bar">
        <Row type="flex" :gutter="16">
          <Col v-show="modalType!=-99999">
              <DatePicker 
                type="daterange" 
                :editable="false"
                :options="searchOptions"
                placement="bottom-start" 
                :placeholder="pickerPlaceholder" 
                v-model="searchModel.modalTimes" 
                format="yyyy-MM-dd"
                @on-change='pickerSearchEvent'
                style="width: 250px"></DatePicker>
          </Col>
          <Col v-show="modalType==1 || modalType==-99999">
              <Select v-model="searchModel.gameId" placeholder="投注游戏" style="width: 100px">
                  <Option v-for="item in gameSelect" :value="item.value" :key="item.key">
                      {{ item.key }}
                  </Option>
              </Select>
          </Col>
          <Col>
              <Button type="ghost" @click="modalSearchEvent"><i class="fa fa-search">查询</i></Button>
          </Col>
        </Row>
    </div>
    <Table :stripe="true" size="small" :columns="modalColumns" :data="modalDatas"></Table>
    <Page :total="modalTotal" :current="modalCurrentPage" :page-size="modalPageSize" @on-change="modalPageChangeEvant" show-total show-elevator class="modal-page" v-show="modalTotal>0"></Page>
</div>
</template>
<script>
export default {
  props: {
    searchOptions: Object,
    searchModel: Object,
    pickerPlaceholder: {
      type: String,
      default: "充值时间"
    },
    modalType: {
      type: Number,
      default: 0
    },
    gameSelect: {
      type: Array,
      default: []
    },
    modalColumns: {
      type: Array,
      default: []
    },
    modalDatas: {
      type: Array,
      default: []
    },
    modalTotal: {
      type: Number,
      default: 0
    },
    modalCurrentPage: {
      type: Number,
      default: 1
    },
    modalPageSize: {
      type: Number,
      default: 5
    }
  },
  methods: {
    pickerSearchEvent: function(times) {
      this.$emit("pickerSearchEvent", times);
    },
    modalSearchEvent: function() {
      this.$emit("modalSearchEvent");
    },
    modalPageChangeEvant: function(page) {
      this.$emit("modalPageChangeEvant", page);
    }
  }
};
</script>
<style scoped>
.modal-search-bar{padding-bottom:5px;padding-top:5px;}
.modal-page{margin-top:10px;}
</style>
