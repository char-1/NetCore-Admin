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
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sortNumber">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="游戏角标类型">
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
			<Col span="6">
				<FormItem label="牌型大小第一位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.player_percent[0]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="牌型大小第二位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.player_percent[1]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="牌型大小第三位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.player_percent[2]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="牌型大小第四位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.player_percent[3]">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="牌型大小第五位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.player_percent[4]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="系统庄牌型大小第一位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sys_percent[0]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="系统庄牌型大小第二位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sys_percent[1]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="系统庄牌型大小第三位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sys_percent[2]">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="系统庄牌型大小第四位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sys_percent[3]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="系统庄牌型大小第五位的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sys_percent[4]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="豹子概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.poke_rate[0].value">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="顺子概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.poke_rate[1].value">
					</InputNumber>
				</FormItem>
			</Col>      
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="金花概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.poke_rate[2].value">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="对子概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.poke_rate[3].value">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="单张概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.poke_rate[4].value">
					</InputNumber>
				</FormItem>
			</Col> 
			<Col span="6">
				<FormItem label="顺金概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.poke_rate[5].value">
					</InputNumber>
				</FormItem>
			</Col>			     
		</Row> 
		<Row type="flex">
			<Col span="6">
				<FormItem label="抽水百分比">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.rate">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="随机红包概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.random_redPack.rate">
					</InputNumber>
				</FormItem>
			</Col>      
			<Col span="6">
				<FormItem label="红包范围最小值">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.random_redPack.min">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="红包范围最大值">
					<InputNumber :max="200000" :min="0" v-model="formItemsBase.config.random_redPack.max">
					</InputNumber>
				</FormItem>
			</Col>
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="上庄最VIP等级">
					<InputNumber :max="9" :min="0" v-model="formItemsBase.config.min_vip">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="上庄最小金币数">
					<InputNumber :max="999999999999" :min="10" v-model="formItemsBase.config.min_gold">
					</InputNumber>
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

