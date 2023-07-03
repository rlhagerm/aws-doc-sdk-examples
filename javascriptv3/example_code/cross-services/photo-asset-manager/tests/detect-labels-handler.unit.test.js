import { describe, it, expect, vi } from "vitest";
import { getHandler } from "../src/functions/detect-labels.js";

describe("detect-labels handler", () => {
  it("should call storeLabels with the key from the event and the labels from detectLabels", async () => {
    /**
     * @type {import("aws-lambda").S3Event}
     */
    const event = {
      Records: [
        {
          s3: {
            bucket: {
              name: "my-bucket",
            },
            object: {
              key: "my_image.jpeg",
            },
          },
        },
      ],
    };

    const store = vi.fn();
    const handler = getHandler({
      detect: () => Promise.resolve(["car", "truck"]),
      store,
    });

    await handler(event);
    expect(store).toHaveBeenCalledWith("my_image.jpeg", ["car", "truck"]);
  });
});
