<template>
<Row class="vm-table vm-panel">
	<Form :label-width="120" inline>
		<Row type="flex">
			<Col span="6">
				<FormItem label="是否开启游戏">
					<i-switch size="large" v-model="formItemsBase.config.isEnable" >
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
					<InputNumber :max="100" :min="0" v-model="formItemsBase.config.sortNumber" >
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
				<FormItem label="777的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[1]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="西瓜的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[2]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="车厘子的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[3]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="葡萄的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[4]">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="草莓的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[5]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="芒果的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[6]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="橙子的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[7]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="苹果的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[8]">
					</InputNumber>
				</FormItem>
			</Col>			
		</Row>
		<Row type="flex">
			<Col span="6">
				<FormItem label="Luck的概率">
					<InputNumber :max="1000" :min="0" v-model="formItemsBase.config.configs[9]">
					</InputNumber>
				</FormItem>
			</Col>
			<Col span="6">
				<FormItem label="抽水比例">
					<InputNumber :max="1" :min="0" v-model="formItemsBase.config.rate">
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

