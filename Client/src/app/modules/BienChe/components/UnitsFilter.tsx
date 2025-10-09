import { FC, useEffect, useState } from 'react'
import { Field, Formik, Form } from 'formik'
import { Button } from 'react-bootstrap'

type Props = {
  onFilter: (values: any) => void
  onReset: () => void
  linhVucs: any[]
  khois: any[]
}



const UnitsFilter: FC<Props> = ({ onFilter, onReset, linhVucs, khois }) => {

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
            <Button type='submit'>Search</Button>
            <Button
              variant='light'
              className='ms-2'
              data-bs-toggle='modal'
              data-bs-target='#kt_modal_1'
            >
              Add
            </Button>
          </div>
        </Form>
      )}
    </Formik>
  )
}

export default UnitsFilter
