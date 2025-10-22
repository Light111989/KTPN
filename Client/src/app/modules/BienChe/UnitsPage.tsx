import React, { useEffect, useState } from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import UnitsFilter from './components/UnitsFilter'
import UnitsTable, { Khoi, LinhVuc } from './components/UnitsTable'
import {
  deleteDonVi,
  exportExcel,
  fetchDonVis,
  fetchKhois,
  fetchLinhVucs,
  searchBienChes,
} from './components/settings/_requests'
import AddUnitModal from './components/AddUnitModal'
import Swal from 'sweetalert2'
import BienCheDetailModal from './components/UnitDetail'
import ExportExcelModal from './components/ExportExcelModal'

// Breadcrumb
const biencheBreadCrumbs: Array<PageLink> = [
  { title: 'Biên Chế', path: '/bien-che/ds-don-vi', isSeparator: false, isActive: false },
]

const UnitsPage: React.FC = () => {
  // Dữ liệu chính
  const [data, setData] = useState<LinhVuc[]>([])
  const [loading, setLoading] = useState(false)

  // Phân trang
  const [page, setPage] = useState(1)
  const [pageSize] = useState(5)
  const [totalRecords, setTotalRecords] = useState(0)

  // Quản lý modal
  const [modalOpen, setModalOpen] = useState(false)         // Add/Edit
  const [mode, setMode] = useState<'add' | 'edit'>('add')
  const [selectedRecord, setSelectedRecord] = useState<any | null>(null)

  const [selectedDetailId, setSelectedDetailId] = useState<string | null>(null) // Detail
  const [showExport, setShowExport] = useState(false)
  // Dùng cho filter + form
  const [linhVucs, setLinhVucs] = useState<LinhVuc[]>([])
  const [khois, setKhois] = useState<Khoi[]>([])


  const handleExport = async (filters: any) => {
    try {
      // gọi API xuất Excel
      await exportExcel(filters)
    } catch (e) {
      console.error(e)
    }
  }
  // ✅ Khi ấn Thêm
  const handleAdd = () => {
    setMode('add')
    setSelectedRecord(null)
    setModalOpen(true)
  }

  // ✅ Khi ấn Sửa
  const handleEdit = (record: any) => {
    setMode('edit')
    setSelectedRecord(record)
    setModalOpen(true)
  }

  // ✅ Khi ấn Xem chi tiết
  const showDetails = (id: string) => {
    setSelectedDetailId(id)
  }

  // ✅ Load dữ liệu
  const loadData = async () => {
    try {
      setLoading(true)
      const res = await fetchDonVis(page, pageSize)
      setData(res.data.items)
      setTotalRecords(res.data.totalRecords)
    } catch (err) {
      console.error('Lỗi khi load Linh Vực:', err)
    } finally {
      setLoading(false)
    }
  }

  // ✅ Xóa
  const handleDelete = async (id: string) => {
    const result = await Swal.fire({
      title: 'Bạn có chắc muốn xóa?',
      text: 'Hành động này không thể hoàn tác!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Có, xóa!',
      cancelButtonText: 'Hủy',
    })

    if (result.isConfirmed) {
      try {
        await deleteDonVi(id)
        setData(prev =>
          prev.map(lv => ({
            ...lv,
            khois: lv.khois.map(khoi => ({
              ...khoi,
              bienChes: khoi.bienChes.filter(bc => bc.id !== id),
            })),
          }))
        )
        Swal.fire('Đã xóa!', 'Bản ghi đã được xóa.', 'success')
      } catch (err) {
        console.error(err)
        Swal.fire('Thất bại!', 'Xóa không thành công.', 'error')
      }
    }
  }

  // ✅ Khi submit form (add hoặc edit)
  const handleSubmitSuccess = () => {
    setModalOpen(false)
    loadData()
  }

  // ✅ Filter
  const handleFilter = async (values: any) => {
    if (!values.tenDonVi && !values.linhVucId && !values.khoiId && !values.effectiveDate) {
      loadData()
    } else {
      const res = await searchBienChes(values)
      setData(res.items)
      setTotalRecords(res.totalRecords)
    }
  }

  // ✅ Lấy dữ liệu LinhVuc & Khoi 1 lần
  useEffect(() => {
    const fetchData = async () => {
      const lv = await fetchLinhVucs()
      const k = await fetchKhois()
      setLinhVucs(lv.data)
      setKhois(k.data)
    }
    fetchData()
  }, [])

  // ✅ Load data khi đổi page
  useEffect(() => {
    loadData()
  }, [page, pageSize])

  return (
    <>
      <PageTitle breadcrumbs={biencheBreadCrumbs}>Danh Sách Đơn Vị</PageTitle>

      <div className="card">
        <div className="card-body">
          {/* Filter */}
          <UnitsFilter
            onFilter={handleFilter}
            linhVucs={linhVucs}
            khois={khois}
            onReset={loadData}
            openModal={handleAdd}
            openModalExport={() => setShowExport(true)} 
          />

          <div className="separator my-6"></div>

          {/* Table */}
          {loading ? (
            <p>Đang tải dữ liệu...</p>
          ) : (
            <UnitsTable
              data={data}
              totalRecords={totalRecords}
              page={page}
              pageSize={pageSize}
              onPageChange={setPage}
              onDelete={handleDelete}
              onUpdate={handleEdit}
              setSelectedId={showDetails}
            />
          )}
        </div>
      </div>

      {/* Modal Add/Edit */}
      <AddUnitModal
        show={modalOpen}
        mode={mode}
        initialData={selectedRecord}
        linhVucs={linhVucs}
        khois={khois}
        onClose={() => setModalOpen(false)}
        onSubmitSuccess={handleSubmitSuccess}
      />

      {/* Modal Detail */}
      <BienCheDetailModal
        bienCheId={selectedDetailId}
        onClose={() => setSelectedDetailId(null)}
      />

      {/* Modal Export Detail */}
      <ExportExcelModal
        show={showExport}
        onClose={() => setShowExport(false)}
        linhVucs={linhVucs}
        khois={khois}
        onExport={handleExport}
      />
    </>
  )
}

export default UnitsPage
