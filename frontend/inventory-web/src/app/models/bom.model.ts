export interface BomItem {
  rawMaterialId: number;
  quantityPerUnit: number;
}

export interface UpdateBomRequest {
  items: BomItem[];
}
