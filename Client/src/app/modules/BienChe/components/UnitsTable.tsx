import React, { useMemo } from 'react'
import { Button } from 'react-bootstrap'
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
  slHD111: number
  effectiveDate: Date
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
  onDelete: (id: string) => void
  onUpdate: (record: BienChe) => void
  setSelectedId: (id: string) => void
}

const UnitsTable: React.FC<UnitsTableProps> = ({
  data,
  onDelete,
  onUpdate,
  setSelectedId,
}) => {
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
      initialState: {
        expanded: Object.fromEntries(
          memoData.map((_, i) => [i, true]) // mở tất cả các row
        ),
      },
    } as TableOptions<LinhVuc> & UseExpandedOptions<LinhVuc>,
    useExpanded
  )


  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
    tableInstance

  return (
    <table {...getTableProps()} className="table table-bordered">
      <thead>
        {headerGroups.map(hg => (
          <tr {...hg.getHeaderGroupProps()} key={hg.id}>
            {hg.headers.map(col => (
              <th {...col.getHeaderProps()} key={col.id}>
                {col.render('Header')}
              </th>
            ))}
          </tr>
        ))}
      </thead>
      <tbody {...getTableBodyProps()}>
        {rows.map(row => {
          prepareRow(row)
          const r = row as ExpandedRow<LinhVuc>
          return (
            <React.Fragment key={r.original.id}>
              <tr {...r.getRowProps()}>
                {r.cells.map(cell => (
                  <td {...cell.getCellProps()} key={cell.column.id}>
                    {cell.render('Cell')}
                  </td>
                ))}
              </tr>

              {r.isExpanded && (
                <tr key={`${r.original.id}-expanded`}>
                  <td colSpan={r.cells.length}>
                    {r.original.khois.map(khoi => {
                      const totals = {
                        slVienChuc: khoi.bienChes.reduce((sum, bc) => sum + bc.slVienChuc, 0),
                        slHopDong: khoi.bienChes.reduce((sum, bc) => sum + bc.slHopDong, 0),
                        slHopDongND: khoi.bienChes.reduce((sum, bc) => sum + bc.slHopDongND, 0),
                        slBoTri: khoi.bienChes.reduce((sum, bc) => sum + bc.slBoTri, 0),
                        slGiaoVien: khoi.bienChes.reduce((sum, bc) => sum + bc.slGiaoVien, 0),
                        slQuanLy: khoi.bienChes.reduce((sum, bc) => sum + bc.slQuanLy, 0),
                        slNhanVien: khoi.bienChes.reduce((sum, bc) => sum + bc.slNhanVien, 0),
                        slHD111: khoi.bienChes.reduce((sum, bc) => sum + bc.slHD111, 0),
                      }

                      return (
                        <div key={khoi.id} className="mb-3">
                          <h6>
                            {khoi.tenKhoi}{' '}
                            <small className="text-muted">
                              (VC: {totals.slVienChuc}, HĐ: {totals.slHopDong}, HĐND:{' '}
                              {totals.slHopDongND}, Bố trí: {totals.slBoTri}, GV:{' '}
                              {totals.slGiaoVien}, QL: {totals.slQuanLy}, NV:{' '}
                              {totals.slNhanVien}, HD111: {totals.slHD111})
                            </small>
                          </h6>

                          <table className="table table-sm table-bordered mb-0">
                            <thead>
                              <tr>
                                <th>Tên đơn vị</th>
                                <th>Thời Gian QĐ</th>
                                <th>SL Viên chức</th>
                                <th>SL Hợp đồng</th>
                                <th>SL Hợp đồng ND</th>
                                <th>SL Bố trí</th>
                                <th>Số Quyết định</th>
                                <th>SL Giáo viên</th>
                                <th>SL Quản lý</th>
                                <th>SL Nhân viên</th>
                                <th>SL HĐ 111</th>
                                <th>Thao tác</th>
                              </tr>
                            </thead>
                            <tbody>
                              {khoi.bienChes.map(bc => (
                                <tr key={bc.id}>
                                  <td>{bc.tenDonVi}</td>
                                  <td>{new Date(bc.effectiveDate).toLocaleDateString()}</td>
                                  <td>{bc.slVienChuc}</td>
                                  <td>{bc.slHopDong}</td>
                                  <td>{bc.slHopDongND}</td>
                                  <td>{bc.slBoTri}</td>
                                  <td>{bc.soQuyetDinh}</td>
                                  <td>{bc.slGiaoVien}</td>
                                  <td>{bc.slQuanLy}</td>
                                  <td>{bc.slNhanVien}</td>
                                  <td>{bc.slHD111}</td>
                                  <td>
                                    <Button
                                      size="sm"
                                      variant="danger"
                                      className="me-2"
                                      onClick={() => onDelete(bc.id)}
                                    >
                                      <i className="fa-solid fa-trash"></i>
                                    </Button>
                                    <Button
                                      size="sm"
                                      variant="success"
                                      className="me-2"
                                      onClick={() => onUpdate(bc)}
                                    >
                                      <i className="fa-solid fa-pencil"></i>
                                    </Button>
                                    <Button
                                      size="sm"
                                      variant="info"
                                      onClick={() => setSelectedId(bc.id)}
                                    >
                                      <i className="fa-solid fa-eye"></i>
                                    </Button>
                                  </td>
                                </tr>
                              ))}

                              <tr className="fw-bold table-light">
                                <td>Tổng</td>
                                <td>-</td>
                                <td>{totals.slVienChuc}</td>
                                <td>{totals.slHopDong}</td>
                                <td>{totals.slHopDongND}</td>
                                <td>{totals.slBoTri}</td>
                                <td>-</td>
                                <td>{totals.slGiaoVien}</td>
                                <td>{totals.slQuanLy}</td>
                                <td>{totals.slNhanVien}</td>
                                <td>{totals.slHD111}</td>
                                <td>-</td>
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
