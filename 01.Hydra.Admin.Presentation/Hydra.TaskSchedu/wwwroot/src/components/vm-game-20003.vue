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
				<FormItem label="不成一线的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[0].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="大小丑的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[1].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="小小丑的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[2].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="皇冠的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[3].per">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">

			<Col span="6">
				<FormItem label="徽章的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[4].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="钥匙的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[5].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="酒杯的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[6].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="戒指的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[7].per">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="帽子的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[8].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="烟斗的概率">
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.config[9].per">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="彩金的概率">
					<InputNumber :max="1" :min="0"  v-model="formItemsBase.config.gifRand">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="抽水比例">
					<InputNumber :max="1" :min="0" v-model="formItemsBase.config.wateRate">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="赢的概率">
					<InputNumber :max="1" :min="0" v-model="formItemsBase.config.winRand">
					</InputNumber>
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
    return {};
  }
};
</script>

