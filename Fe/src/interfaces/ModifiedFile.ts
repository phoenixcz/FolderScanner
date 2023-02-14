import type { modifiedFileType } from "@/enum/modifiedFileType";

export interface ModifiedFile {
  fullName: string;
  version: number;
  type: modifiedFileType;
}
