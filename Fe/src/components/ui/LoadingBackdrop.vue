<script>
import { defineComponent } from "vue";
import BackdropOverlay from "@/components/ui/BackdropOverlay.vue";
import ProgressSpinner from "@/components/ui/ProgressSpinner.vue";

export default defineComponent({
  components: { ProgressSpinner, BackdropOverlay },
});
</script>

<template>
  <BackdropOverlay>
    <ProgressSpinner />
  </BackdropOverlay>
</template>

<style scoped lang="scss"></style>
