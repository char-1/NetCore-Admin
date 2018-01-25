<template>
<Row class="vm-table vm-panel">
	<Form :label-width="120" inline>
		<Row type="flex">
			<Col span="6">
				<FormItem label="是否开启游戏">
					<i-switch size="large" v-model="formItemsBase.config.isEnable">
						<span slot="open">
							开启
						</span>
						<span slot="close">
							关闭
						</span>
					</i-switch>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="大厅游戏显示排序">
					<InputNumber :max="100" :min="0"  v-model="formItemsBase.config.sortNumber">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="游戏按钮角标">
          <Select v-model="formItemsBase.config.gameCorner" style="width:100px">
              <Option v-for="item in gameCorner" :value="item.value" :key="item.key">{{ item.key }}</Option>
          </Select>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="游戏按钮名称">
          <Input v-model="formItemsBase.config.btnPanel" placeholder="请输入游戏按钮名称" :readonly="true"></Input>
				</FormItem>
			</Col>      
		</Row>
		<Row type="flex">
			<Col span="24">
				<FormItem label="下注单注">
					<Tag v-for="item in formItemsBase.config.down_bet_arr" :key="item" :name="item" closable @on-close="tagRemoveEvent">{{ item }}</Tag>
					<Button icon="ios-plus-empty" type="dashed" size="small" @click="tagAddEvent">添加单注</Button>
				</FormItem>	
			</Col>
		</Row>		
	</Form>
</Row>
</template>
<script>
export default {
  props: {
    formItemsBase: {
      type: Object
    },
    gameCorner: {
      type: Array
    }
  },
  date() {
    return {
      singleValue: 100
    };
  },
  methods: {
    tagRemoveEvent: function(event, name) {
      this.$emit("tagRemoveEvent", name);
    },
    tagAddEvent: function() {
      this.$Modal.confirm({
        render: h => {
          return h("InputNumber", {
            props: {
              value: this.singleValue,
              autofocus: true,
              min: 1
            },
            on: {
              input: val => {
                this.singleValue = val;
              }
            }
          });
        },
        onOk: () => {
          if (this.singleValue < 0 || this.singleValue > 99999999999) return;
          this.$emit("tagAddEvent", this.singleValue);
        }
      });
    }
  }
};
</script>

