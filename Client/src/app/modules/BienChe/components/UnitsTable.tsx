import React, { useMemo } from 'react'
import {
  useTable,
  useExpanded,
  Column,
  Row,
  TableOptions,
  UseExpandedRowProps,
  UseExpandedOptions,
} from 'react-table'

// ------------------- Kiểu dữ liệu -------------------
export type LinhVuc = {
  id: string
  tenLinhVuc: string
  khois: Khoi[]
}

export type Khoi = {
  id: string
  tenKhoi: string
  bienChes: BienChe[]
}

export type BienChe = {
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
  khoiId: string
  linhVucId: string
}


// ------------------- Helper type -------------------
type ExpandedRow<T extends object> = Row<T> & UseExpandedRowProps<T>

// ------------------- Props -------------------
type UnitsTableProps = {
  data: LinhVuc[]
  totalRecords: number
  page: number
  pageSize: number
  onPageChange: (page: number) => void
  onDelete: (id: string) => void // 👈 chỉ nhận id Biên chế
  onUpdate: (record: any) => void
  onAddClick?: () => void
}

const UnitsTable: React.FC<UnitsTableProps> = ({ data, onDelete, onUpdate }) => {
  const columns: Column<LinhVuc>[] = useMemo(
    () => [
      {
        Header: 'Tên lĩnh vực',
        accessor: 'tenLinhVuc',
        Cell: ({ row }) => {
          const r = row as ExpandedRow<LinhVuc>
          return (
            <span {...r.getToggleRowExpandedProps()} style={{ cursor: 'pointer' }}>
              {r.isExpanded ? '▼' : '▶'} {r.original.tenLinhVuc}
            </span>
          )
        },
      },
    ],
    []
  )
  const memoData = useMemo(() => data, [data])
  const tableInstance = useTable<LinhVuc>(
    {
      columns,
      data: memoData,
    } as TableOptions<LinhVuc> & UseExpandedOptions<LinhVuc>,
    useExpanded
  )

  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
    tableInstance

  return (
    <table {...getTableProps()} className="table table-bordered">
      <thead>
        {headerGroups.map((hg) => (
          <tr {...hg.getHeaderGroupProps()}>
            {hg.headers.map((col) => (
              <th {...col.getHeaderProps()}>
                {col.render('Header')}
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody {...getTableBodyProps()}>
        {rows.map((row) => {
          prepareRow(row)
          const r = row as ExpandedRow<LinhVuc>
          return (
            <React.Fragment key={r.original.id}>
              <tr {...r.getRowProps()}>
                {r.cells.map((cell) => (
                  <td {...cell.getCellProps()}>
                    {cell.render('Cell')}
                  </td>
                ))}
              </tr>

              {r.isExpanded && (
                <tr>
                  <td colSpan={r.cells.length}>
                    {r.original.khois.map((khoi) => {
                      const totals = {
                        slVienChuc: khoi.bienChes.reduce((sum, bc) => sum + bc.slVienChuc, 0),
                        slHopDong: khoi.bienChes.reduce((sum, bc) => sum + bc.slHopDong, 0),
                        slHopDongND: khoi.bienChes.reduce((sum, bc) => sum + bc.slHopDongND, 0),
                        slBoTri: khoi.bienChes.reduce((sum, bc) => sum + bc.slBoTri, 0),
                        slGiaoVien: khoi.bienChes.reduce((sum, bc) => sum + bc.slGiaoVien, 0),
                        slQuanLy: khoi.bienChes.reduce((sum, bc) => sum + bc.slQuanLy, 0),
                        slNhanVien: khoi.bienChes.reduce((sum, bc) => sum + bc.slNhanVien, 0),
                      }

                      return (
                        <div key={khoi.id} className="mb-3">
                          <h6>
                            {khoi.tenKhoi} &nbsp;
                            <small className="text-muted">
                              (Viên chức: {totals.slVienChuc},
                              Hợp đồng: {totals.slHopDong},
                              HĐND: {totals.slHopDongND},
                              Bố trí: {totals.slBoTri},
                              GV: {totals.slGiaoVien},
                              QL: {totals.slQuanLy},
                              NV: {totals.slNhanVien})
                            </small>
                          </h6>

                          <table className="table table-sm table-bordered mb-0">
                            <thead>
                              <tr>
                                <th>Tên đơn vị</th>
                                <th>SL Viên chức</th>
                                <th>SL Hợp đồng</th>
                                <th>SL Hợp đồng ND</th>
                                <th>SL Bố trí</th>
                                <th>Số Quyết định</th>
                                <th>SL Giáo viên</th>
                                <th>SL Quản lý</th>
                                <th>SL Nhân viên</th>
                              </tr>
                            </thead>
                            <tbody>
                              {khoi.bienChes.map((bc) => (
                                <tr key={bc.id}>
                                  <td>{bc.tenDonVi}</td>
                                  <td>{bc.slVienChuc}</td>
                                  <td>{bc.slHopDong}</td>
                                  <td>{bc.slHopDongND}</td>
                                  <td>{bc.slBoTri}</td>
                                  <td>{bc.soQuyetDinh}</td>
                                  <td>{bc.slGiaoVien}</td>
                                  <td>{bc.slQuanLy}</td>
                                  <td>{bc.slNhanVien}</td>
                                  <td>
                                    <button
                                      className="btn btn-icon btn-primary fa-solid fa-trash fs-8 me-4"
                                      onClick={() => onDelete(bc.id)}
                                    >
                                    </button>
                                    <button
                                      className="btn btn-icon btn-success fa-solid fa-pencil fs-8 me-4"
                                      onClick={() => onUpdate(bc)}
                                    >
                                    </button>
                                  </td>
                                </tr>
                              ))}
                              {/* Hàng tổng cuối bảng */}
                              <tr className="fw-bold table-light ">
                                <td>Tổng</td>
                                <td>{totals.slVienChuc}</td>
                                <td>{totals.slHopDong}</td>
                                <td>{totals.slHopDongND}</td>
                                <td>{totals.slBoTri}</td>
                                <td>-</td>
                                <td>{totals.slGiaoVien}</td>
                                <td>{totals.slQuanLy}</td>
                                <td>{totals.slNhanVien}</td>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                      )
                    })}
                  </td>
                </tr>
              )}

            </React.Fragment>
          )
        })}
      </tbody>

    </table>
  )
}

export default UnitsTable
