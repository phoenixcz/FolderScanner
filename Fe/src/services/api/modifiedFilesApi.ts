import axios, { AxiosError } from "axios";
import type { ModifiedFile } from "@/interfaces/ModifiedFile";

export class ModifiedFilesApi {
  resource = "/modifiedFiles";

  async getModifiedFiles(path: string): Promise<Array<ModifiedFile>> {
    try {
      const result = await axios.get(
        `${this.resource}/${encodeURIComponent(path)}`
      );
      return result.data;
    } catch (e) {
      if (e instanceof AxiosError) {
        throw new Error(e.request.response || e.message);
      }
      throw new Error("Error while getting modified files");
    }
  }
}

export const modifiedFilesApi = new ModifiedFilesApi();
