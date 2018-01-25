<template>
    <div>
        <Row v-for="item in row" :key="item._id">
            <Col span="12" class="expand-col">
                <span class="expand-key">投注项: </span>
                <span class="expand-value">{{ item._id }}</span>
            </Col>
            <Col span="12" class="expand-col">
                <span class="expand-key">投注金额: </span>
                <span class="expand-value">{{ FormatMoney(item.score) }}</span>
            </Col>
        </Row>
    </div>
</template>
<script>
import { FormatMoney } from "../data/utils";
export default {
  props: {
    row: Array
  },
  methods: {
    FormatMoney: function(number) {
      return FormatMoney(number);
    }
  }
};
</script>
<style scoped>
.expand-col {
  display: block;
  width: 50%;
  border-radius: 1px;
  padding: 2px 10px;
  margin: 2px 0;
  background: #fff;
}
</style>