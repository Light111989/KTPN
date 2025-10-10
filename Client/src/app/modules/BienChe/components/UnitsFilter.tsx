import { FC, useEffect, useState } from 'react'
import { Field, Formik, Form } from 'formik'
import { Button } from 'react-bootstrap'
import { exportExcel } from './settings/_requests'
import { Modal } from 'bootstrap'

type Props = {
  onFilter: (values: any) => void
  onReset: () => void
  openModal: () => void
  linhVucs: any[]
  khois: any[]
}

// const openModal = () => {
//   const modalEl = document.getElementById('kt_modal_1')
//   if (modalEl) {
//     const modal = new Modal(modalEl) // luôn tạo instance mới
//     modal.show()
//   }
// }

const UnitsFilter: FC<Props> = ({ onFilter, openModal, linhVucs, khois }) => {

  return (
    <Formik
      initialValues={{ linhVucId: '', khoiId: '', tenDonVi: '' }}
      onSubmit={(values) => onFilter(values)}
    >
      {() => (
        <Form className='row g-3 align-items-center'>
          <div className='col-auto'>
            <Field as='select' name='linhVucId' className='form-select'>
              <option value=''>-- Chọn lĩnh vực --</option>
              {linhVucs.map((lv) => (
                <option key={lv.linhVucId} value={lv.linhVucId}>
                  {lv.tenLinhVuc}
                </option>
              ))}
            </Field>
          </div>
          <div className='col-auto'>
            <Field as='select' name='khoiId' className='form-select'>
              <option value=''>-- Chọn Khối --</option>
              {khois.map((k) => (
                <option key={k.khoiId} value={k.khoiId}>
                  {k.tenKhoi}
                </option>
              ))}
            </Field>
          </div>
          <div className='col-auto'>
            <Field
              name='tenDonVi'
              className='form-control'
              placeholder='Nhập tên đơn vị'
            />
          </div>
          <div className='col-auto'>
            <Button type='submit'
              variant='light'
              className='ms-2 btn btn-primary'

            >
              <span className="bi bi-search"></span>
              Search
            </Button>
            <Button

              variant='light'
              className='ms-2 btn btn-info'
              onClick={openModal}
            >
              <span className="bi bi-file-earmark-plus"></span>
              Add
            </Button>
            <Button
              onClick={() => exportExcel()}
              className=" ms-2 btn btn-success">
              <span className=" bi bi-file-earmark-spreadsheet fs-6 me-2"></span>
              Excel
            </Button>
          </div>
        </Form>
      )}
    </Formik>
  )
}

export default UnitsFilter
