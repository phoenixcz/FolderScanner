import { describe, it, expect } from "vitest";

import { mount } from "@vue/test-utils";
import DisplayMessage from "../DisplayMessage.vue";

describe("DisplayMessage", () => {
  it("renders properly", () => {
    const text = "Hello!";
    const wrapper = mount(DisplayMessage, { props: { value: text } });
    expect(wrapper.text()).toContain(text);
  });
});
