<template>
<Row class="vm-table vm-panel">
    <Row type="flex" style="padding-bottom:10px;">
      <Col span="24">
				<Button icon="ios-plus-empty" type="dashed" size="small" @click="ctrlAddEvent">添加时间段</Button>
			</Col>
    </Row>  
		<Row type="flex" v-for="(item,index) in timeCtrl" :key="index">
			<Col span="5">
				<FormItem label="开始时间/时">
          <Select :transfer="true" v-model="this._.find(formItem.data.games, {id: tabGameId}).timectl[index][0]" style="width:100px;">
              <Option v-for="item in this._.range(0, 24)" :value="item" :key="item">{{ item }} 点</Option>
          </Select>
				</FormItem>
			</Col>
			<Col span="5">
				<FormItem label="最小机器人数">
					<InputNumber :max="100000000000" :min="0" v-model="this._.find(formItem.data.games, {id: tabGameId}).timectl[index][1]">
          </InputNumber>
				</FormItem>
			</Col>
			<Col span="5">
				<FormItem label="最大机器人数">
					<InputNumber :max="100000000000" :min="0" v-model="this._.find(formItem.data.games, {id: tabGameId}).timectl[index][2]">
          </InputNumber>
				</FormItem>
			</Col>
			<Col span="5">
				<FormItem label="下注概率">
					<InputNumber :max="100" :min="0" v-model="this._.find(formItem.data.games, {id: tabGameId}).timectl[index][3]">
                    </InputNumber>
				</FormItem>
			</Col> 	
			<Col span="4">
				<Button icon="ios-plus-empty" type="dashed" size="small" @click="ctrlRemoveEvent(index,tabGameId)">删除时间段</Button>
			</Col>
		</Row>
</Row>
</template>
<script>
export default {
  props: {
    formItem: {
      type: Object
    },
    timeCtrl: {
      type: Array
    },
    tabGameId: Number
  },
  date() {
    return {
      singleValue: 100
    };
  },
  methods: {
    ctrlRemoveEvent: function(index,tabGameId) {
      this.$emit("ctrlRemoveEvent", index,tabGameId);
    },
    ctrlAddEvent: function() {
      this.$emit("ctrlAddEvent");
    }
  }
};
</script>

