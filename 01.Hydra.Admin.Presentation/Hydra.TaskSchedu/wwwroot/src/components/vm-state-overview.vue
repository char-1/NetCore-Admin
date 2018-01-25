<template>
  <Row class="vm-state-overview" type="flex">
    <Col span="10" class="symbol" :style="{ backgroundColor: color }">
      <i :class="icon"></i>
    </Col>
    <Col span="14" class="value">
      <h1 class="count">{{ count }}</h1>
       <Tooltip :content="tooltip" placement="top">
         <label>{{ title }}</label>
      </Tooltip>
    </Col>
  </Row>
</template>

<script>
export default {
  name: "StateOverView",
  props: ["color", "icon", "title", "count","tooltip"]
};
</script>
