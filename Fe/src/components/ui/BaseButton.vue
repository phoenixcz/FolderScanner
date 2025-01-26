<script setup lang="ts">
defineProps({
  label: {
    type: String,
    required: true,
  },
  type: {
    type: String as () => "button" | "submit" | "reset",
    default: "button",
  },
  dataTest: {
    type: String,
    default: "base-button",
  },
});

const emit = defineEmits(["click"]);

const handleClick = () => {
  emit("click");
};
</script>

<template>
  <button
    class="base-button"
    :type="type"
    @click="handleClick"
    :data-test="dataTest"
  >
    {{ label }}
  </button>
</template>

<style scoped lang="scss">
.base-button {
  font: inherit;
  cursor: pointer;
  padding: 0 1rem;
  border: 1px solid darkgreen;
  background-color: hsla(160, 100%, 37%, 1);
  color: white;
  border-radius: 12px;
  height: 2.5rem;
  transition: background-color 0.2s;

  &:hover,
  &:active {
    background-color: hsla(160, 100%, 37%, 0.9);
  }
}
</style>
