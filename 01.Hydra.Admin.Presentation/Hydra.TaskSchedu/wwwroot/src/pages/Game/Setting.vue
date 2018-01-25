<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
         
         <Tabs type="card" @on-click="tabsClickEvent" :animated="false" style="height:600px;">
           <Alert type="warning" show-icon>修改或新增配置后,请点击保存按钮！</Alert>
              <TabPane v-for="item in gameTabs" :label="item.name" :key="item.id" :name="item.id">
                <VmGame20011 v-if="item.id=='20011'" :gameCorner="gameCorner" :formItemsBase="formItemsBase" v-on:tagRemoveEvent="tagRemoveEvent" v-on:tagAddEvent="tagAddEvent"></VmGame20011>
                <VmGame10021 v-if="item.id=='10021'" :gameCorner="gameCorner" :formItemsBase="formItemsBase" v-on:tagRemoveEvent="tagRemoveEvent" v-on:tagAddEvent="tagAddEvent"></VmGame10021>
                <VmGame10031 v-if="item.id=='10031'" :gameCorner="gameCorner" :formItemsBase="formItemsBase" v-on:tagRemoveEvent="tagRemoveEvent" v-on:tagAddEvent="tagAddEvent"></VmGame10031>
                <VmGame30011 v-if="item.id=='30011'" :gameCorner="gameCorner" :formItemsBase="formItemsBase" v-on:tagRemoveEvent="tagRemoveEvent" v-on:tagAddEvent="tagAddEvent"></VmGame30011>
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
import VmGame20011 from "@/components/vm-game-20011";
import VmGame10021 from "@/components/vm-game-10021";
import VmGame10031 from "@/components/vm-game-10031";
import VmGame30011 from "@/components/vm-game-30011";
import { HTTP_URL_API } from "../../data/api";
import { HttpGet, HttpPost, SerializeForm, MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "游戏配置",
      gameTabs: [
        { id: "20011", name: "金鲨银鲨" },
        { id: "10021", name: "百人金花" },
        { id: "10031", name: "百人牛牛" },
        { id: "30011", name: "走地德州" }
      ],
      gameCorner: [
        { key: "无", value: 0 },
        { key: "新游戏", value: 1 },
        { key: "热门游戏", value: 2 },
        { key: "推荐游戏", value: 3 }
      ],
      showBtnLoading: false,
      //公共属性
      formItemsBase: {
        _id: 0,
        config: {},
        gameid: 0
      },
      currentGameId: 0,
      saveData: {} //提交数据
    };
  },
  methods: {
    saveEvent: function() {
      if (this.formItemsBase && this.formItemsBase.gameid != 0) {
        this.showBtnLoading = true;
        delete this.formItemsBase.sign;
        HttpPost(
          HTTP_URL_API.SET_GAME_CONFIG,
          MakeSign(this.formItemsBase),
          "application/json;charset=utf-8"
        )
          .then(result => {
            if (
              result.data.data &&
              result.data.data == "success"
            ) {
              this.$Notice.success({
                title: "操作成功"
              });
            } else
              this.$Notice.error({
                title: result.data.result.message
              });
          })
          .then(() => {
            setTimeout(() => {
              this.showBtnLoading = false;
            }, 800);
          });
      }
    },
    tabsClickEvent: function(name) {
      if (name) {
        HttpGet(HTTP_URL_API.GET_GAME_CONFIG, { gameid: name }).then(result => {
          if (result && result.data.data.config) {
            this.formItemsBase.config = result.data.data.config;
            this.formItemsBase._id = Number(name);
            this.formItemsBase.gameid = Number(name);
          }
        });
      }
    },
    tagRemoveEvent: function(_name) {
      let single_array = this.formItemsBase.config.down_bet_arr;
      const index = single_array.indexOf(_name);
      single_array.splice(index, 1);
    },
    tagAddEvent: function(_value) {
      let single_array = this.formItemsBase.config.down_bet_arr;
      if (single_array.length >= 6) {
        this.$Notice.error({
          title: "超过最大限制6个"
        });
        return;
      }
      if (this._.indexOf(single_array, _value) < 0) single_array.push(_value);
      this.formItemsBase.config.down_bet_arr = this._.sortBy(
        single_array,
        item => {
          return item;
        }
      );
    }
  },
  components: {
    VmGame20011,
    VmGame10021,
    VmGame10031,
    VmGame30011
  },
  mounted: function() {
    this.tabsClickEvent(this.gameTabs[0].id);
  }
};
</script>