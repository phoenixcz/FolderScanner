<script setup lang="ts"></script>

<template>
  <header>
    <div class="header">
      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/limitations">Limitations</RouterLink>
      </nav>
    </div>
  </header>
</template>

<style scoped lang="scss">
.header {
  width: 100%;
  height: 50px;

  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 var(--default-margin-gap);

  a {
    text-decoration: none;
    color: hsla(160, 100%, 37%, 1);
    transition: 0.4s;
    padding: 15px;

    &:hover {
      background-color: hsla(160, 100%, 37%, 0.2);
    }
  }
}
</style>
