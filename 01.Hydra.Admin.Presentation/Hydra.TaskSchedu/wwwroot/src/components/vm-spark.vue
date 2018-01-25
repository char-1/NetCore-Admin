<template>
    <div>
        <Row>
            <Alert type="warning" show-icon>修改或新增配置后,请点击保存按钮！</Alert>
            <Form :model="formModel" :label-width="50">
                <FormItem label="资金池" prop="goldpool">
                    <InputNumber :max="99999999999" :min="0" v-model="formModel.goldpool" placeholder="输入资金池" style="width:300px"></InputNumber>
                </FormItem>
            </Form>
        </Row>
        <div class="edit">
            <Button @click="modalShow = true" ><i class="fa fa-plus"></i>添加配置</Button>
        </div>
        <Table :stripe="showStripe" :size="tableSize" :columns="columns" :data="tableConfigs"></Table>
        <Modal
            v-model="modalShow"
            :title="modalTitle"
            :mask-closable="false"
            ok-text="确定"
            cancel-text="取消"
            v-on:on-ok="modalSaveEvent">
            <Form :label-width="120" ref="formModel" :model="formModel" :rules="ruleValidate">
                <FormItem label="奖池最小金币" prop="minold">
                    <InputNumber v-model="formModel.configs.minGold"  placeholder="请输入奖池最小金币" style="width:200px"></InputNumber>
                </FormItem>
                <FormItem label="奖池最大金币" prop="maxGold">
                    <InputNumber v-model="formModel.configs.maxGold"  placeholder="请输入奖池最大金币" style="width:200px"></InputNumber>
                </FormItem>
                <FormItem label="平台赢的概率" prop="rate">
                    <InputNumber v-model="formModel.configs.rate" :min="-1" :max="100" placeholder="请输入平台赢的概率" style="width:200px"></InputNumber>
                </FormItem>                
            </Form>            
        </Modal>          
    </div>
</template>
<script>
export default {
  props: {
    columns: Array,
    tableConfigs: Array,
    ruleValidate: Object,
    formModel: Object,
    modalTitle: {
      type: String,
      default: "配置新增"
    }
  },
  data() {
    return {
      showStripe: true,
      tableSize: "small",
      modalShow:false
    };
  },
  methods: {
    modalSaveEvent: function() {
      this.$emit("modalSaveEvent", this.formModel);
    }
  }
};
</script>