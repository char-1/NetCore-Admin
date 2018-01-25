<template>
<Row class="vm-table vm-panel">
    <div class="panel-heading">
      {{ title }}
    </div>
    <div class="panel-body">
         <Tabs type="card" @on-click="tabsClickEvent" :animated="false" style="height:550px;">
                <TabPane v-for="item in gameTabs" :label="item.name" :key="item.id" :name="item.id">
                    <VmSpark 
                    :columns="columns" 
                    :formModel="formModel"
                    :tableConfigs="tableConfigs" 
                    :ruleValidate="ruleValidate" 
                    :modalTitle="modalTitle"
                    v-on:modalSaveEvent="modalSaveEvent"></VmSpark>
                </TabPane>
        </Tabs>
        <Row type="flex" justify="end" class="code-row-bg">
          <Button type="primary" @click="saveClickEvent()" :loading="showBtnLoading" icon="checkmark">
            保存
          </Button>
        </Row>          
    </div>  
</Row>
</template>
<script>
import { HTTP_URL_API } from "../../data/api";
import { HttpPost, HttpGet, SerializeForm, MakeSign } from "../../data/utils";
import VmSpark from "@/components/vm-spark";
export default {
  data() {
    return {
      title: "Spark配置",
      modalTitle: "配置新增",
      gameTabs: [
        { id: "20011", name: "金鲨银鲨" },
        { id: "10021", name: "百人金花" },
        { id: "10031", name: "百人牛牛" },
        { id: "30011", name: "走地德州" }
      ],
      currentGameId: 0,
      columns: [
        {
          type: "index",
          width: 60,
          align: "center"
        },
        {
          title: "奖池最小金币",
          key: "minGold"
        },
        {
          title: "奖池最大金币",
          key: "maxGold"
        },
        {
          title: "平台赢的概率",
          key: "rate"
        },
        {
          title: "操作",
          key: "action",
          width: 100,
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
                  style: {
                    marginRight: "2px"
                  },
                  on: {
                    click: () => {
                      this.tableConfigs.splice(params.index, 1);
                    }
                  }
                },
                "删除"
              )
            ]);
          }
        }
      ],
      formModel: {
        _id: "",
        goldpool: 0,
        configs: {
          minGold: 0,
          maxGold: 0,
          rate: 0
        }
      },
      tableConfigs: [],
      showBtnLoading: false,
      ruleValidate: {
        minold: [
          {
            type: "number",
            required: true,
            message: "奖池最小金币不能为空",
            trigger: "blur"
          }
        ],
        maxGold: [
          {
            type: "number",
            required: true,
            message: "奖池最大金币不能为空",
            trigger: "blur"
          }
        ],
        rate: [
          {
            type: "number",
            required: true,
            message: "平台赢的概率不能为空",
            trigger: "blur"
          }
        ]
      }
    };
  },
  methods: {
    modalSaveEvent: function(_data) {
      if (_data) {
        // _data.configs._id = this.uuid.v1();
        this.tableConfigs.push(_data.configs);
        // this.formModel.goldpool = 0;
        this.formModel.configs = {
          minGold: 0,
          maxGold: 0,
          rate: 0
        };
      }
    },
    tabsClickEvent: function(name) {
      this.formModel._id = name;
      if (name) {
        HttpGet(HTTP_URL_API.GET_SPARK_CONFIG, {
          gameId: name
        }).then(result => {
          if (result.data.data.spark) {
            this.tableConfigs = result.data.data.spark.configs;
            this.formModel.goldpool = result.data.data.spark.goldpool;
          } else {
            this.tableConfigs = [];
            this.formModel.goldpool = 0;
          }
        });
      }
    },
    saveClickEvent: function() {
      let request = {
        _id: this.formModel._id,
        goldpool: this.formModel.goldpool,
        configs: this.tableConfigs
      };
      HttpPost(
        HTTP_URL_API.SET_SPARK_CONFIG,
        MakeSign(request),
        "application/json;charset=utf-8"
      ).then(result => {
        if (result && result.data.data == "success") {
          this.$Message.success("提交成功");
        } else this.$Message.error("提交失败");
      });
    }
  },
  components: { VmSpark },
  mounted: function() {
    this.tabsClickEvent(this.gameTabs[0].id);
  }
};
</script>