<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
        <Alert type="warning" show-icon>修改或新增配置后,请点击保存按钮！</Alert>
        <Row type="flex" justify="end" class="code-row-bg">
          <Button type="primary" @click="saveEvent()" :loading="showBtnLoading" icon="checkmark">
            保存
          </Button>
        </Row>         
        <Form :label-width="120" inline>
            <Row type="flex">
                <Col span="6">
                    <FormItem label="开启机器人">
                        <Select v-model="formItem.data.start" style="width:100px">
                            <Option v-for="item in selectOptions" :value="item.value" :key="item.key">{{ item.key }}</Option>
                        </Select>                        
                    </FormItem>
                </Col>
            </Row>
            <Row type="flex">
                <Col span="6">
                    <FormItem label="最小退出金币数">
                        <InputNumber :max="100000000000" :min="100" v-model="formItem.data.minExitGameTakeGold">
                            </InputNumber>
                    </FormItem>
                </Col>
                <Col span="6">
                    <FormItem label="随机金币最大值">
                        <InputNumber :max="100000000000" :min="100" v-model="formItem.data.maxInitGoldRandom">
                            </InputNumber>
                    </FormItem>
                </Col>
                <Col span="6">
                    <FormItem label="随机金币最小值">
                        <InputNumber :max="100000000000" :min="100" v-model="formItem.data.minInitGoldRandom">
                        </InputNumber>
                    </FormItem>
                </Col>
                <Col span="6">
                    <FormItem label="最小发钱金币数">
                        <InputNumber :max="100000000000" :min="100" v-model="formItem.data.minTakeGold">
                            </InputNumber>
                    </FormItem>
                </Col>                
            </Row>                          
            <Tabs type="card" @on-click="tabsClickEvent" :animated="false" >
            <TabPane v-for="item in gameTabs" :label="item.name" :key="item.id" :name="item.id">
                <VmRobot :formItem="formItem" :timeCtrl="timeCtrl" :tabGameId="tabGameId" 
                v-on:ctrlRemoveEvent="ctrlRemoveEvent"
                v-on:ctrlAddEvent="ctrlAddEvent"></VmRobot>
            </TabPane>           
            </Tabs>
        </Form>                     
    </div>
</Row>
</template>
<script>
import VmRobot from "@/components/vm-robot";
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm, MakeSign } from "../../data/utils";
export default {
  data() {
    return {
      title: "机器人配置",
      gameTabs: [
        { id: "20011", name: "金鲨银鲨" },
        { id: "10021", name: "百人金花" },
        { id: "10031", name: "百人牛牛" },
        { id: "30011", name: "走地德州" }
      ],
      tabGameId: 20011,
      selectOptions: [{ key: "是", value: 1 }, { key: "否", value: 0 }],
      showBtnLoading: false,
      //公共属性
      formItem: {
        _id: "robotConfig",
        data: {
          start: 1,
          minExitGameTakeGold: 500000,
          maxInitGoldRandom: 100000000,
          minInitGoldRandom: 10000000,
          minTakeGold: 10000000,
          games: [
            {
              id: 10031,
              timectl: [
                [0, 20, 25, 50],
                [8, 40, 45, 50],
                [14, 60, 65, 50],
                [20, 60, 65, 50]
              ]
            }
          ]
        }
      },
      timeCtrl: [],
      saveData: {} //提交数据
    };
  },
  methods: {
    saveEvent: function() {
      if (this.formItem) {
        this.showBtnLoading = true;
        delete this.formItem.sign;
        HttpPost(
          HTTP_URL_API.SET_SINGLE_CONFIG,
          MakeSign(this.formItem),
          "application/json;charset=utf-8"
        )
          .then(result => {
            if (result && result.data.data.msg == "success") {
              this.$Notice.success({
                title: "操作成功"
              });
            } else
              this.$Notice.error({
                title: "操作失败"
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
        let gameId = Number(name);
        this.tabGameId = gameId;
        let gameCtrl = this._.find(this.formItem.data.games, {
          id: gameId
        });
        if (gameCtrl) this.timeCtrl = gameCtrl.timectl;
        else this.timeCtrl = [];
      }
    },
    initConfigEvent: function() {
      HttpGet(HTTP_URL_API.GET_SINGLE_CONFIG, {
        _id: "robotConfig"
      }).then(result => {
        if (result && result.data.data) {
          this.formItem.data = result.data.data;
          let game = this._.find(this.formItem.data.games, {
            id: this.tabGameId
          });
          if (game && game.timectl.length > 0)
            this.timeCtrl = this._.find(this.formItem.data.games, {
              id: this.tabGameId
            }).timectl;
        }
      });
    },
    ctrlRemoveEvent: function(index, tabGameId) {
      let game = this._.find(this.formItem.data.games, {
        id: tabGameId
      });
      if (game && game.timectl.length > 0) {
        game.timectl.splice(index, 1);
      }
    },
    ctrlAddEvent: function() {
      let game = this._.find(this.formItem.data.games, {
        id: this.tabGameId
      });
      if (game && game.timectl.length > 0)
        this._.find(this.formItem.data.games, {
          id: this.tabGameId
        }).timectl.push([0, 0, 0, 0]);
      else {
        this.formItem.data.games.push({
          id: this.tabGameId,
          timectl: [[0, 0, 0, 0]]
        });
        this.timeCtrl = this._.find(this.formItem.data.games, {
          id: this.tabGameId
        }).timectl;
      }
    }
  },
  components: {
    VmRobot
  },
  mounted: function() {
    this.initConfigEvent();
  }
};
</script>