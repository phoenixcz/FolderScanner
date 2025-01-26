<script setup lang="ts">
defineProps({
  modelValue: {
    type: String,
    required: true,
  },
  placeholder: {
    type: String,
    default: "",
  },
  dataTest: {
    type: String,
    default: "base-input",
  },
});

const emit = defineEmits(["update:modelValue"]);

const handleInput = (event: Event) => {
  const target = event.target as HTMLInputElement;
  emit("update:modelValue", target.value);
};
</script>

<template>
  <input
    class="base-input"
    :placeholder="placeholder"
    :value="modelValue"
    @input="handleInput"
    :data-test="dataTest"
  />
</template>

<style scoped lang="scss">
.base-input {
  font: inherit;
  padding: 0.5rem;
  border-radius: 6px;
  border: 1px solid #ccc;
  width: 100%;
  height: 2.5rem;
  margin-bottom: 1rem;
  transition: border-color 0.2s;

  &:focus {
    outline: none;
    border-color: darkgreen;
  }
}
</style>
