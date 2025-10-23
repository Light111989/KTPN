export type DonVi = {
  id: string
  tenDonVi: string
  slVienChuc: number
  slHopDong: number
  slHopDongND: number
  slBoTri: number
  soQuyetDinh: string
  slGiaoVien: number
  slQuanLy: number
  slNhanVien: number
  slHD111: number
  effectiveDate : Date

  // enrich thêm từ tree
  khoiId: string
  tenKhoi: string
  linhVucId: string
  tenLinhVuc: string
}
// export function flattenTree(tree: any[]): DonVi[] {
//   const result: DonVi[] = []

//   tree.forEach((lv) => {
//     lv.khois.forEach((khoi: any) => {
//       khoi.bienChes.forEach((bc: any) => {
//         result.push({
//           ...bc,
//           tenKhoi: khoi.tenKhoi,
//           tenLinhVuc: lv.tenLinhVuc,
//         })
//       })
//     })
//   })

//   return result
// }