import { FC, useEffect, useState } from 'react'
import { Field, Formik, Form } from 'formik'
import { Button } from 'react-bootstrap'
import { exportExcel } from './settings/_requests'
import { Modal } from 'bootstrap'
import { useFormik } from "formik";
import DatePicker from "react-datepicker";
import "../../../../../node_modules/react-datepicker/dist/react-datepicker.css";


import Flatpickr from "react-flatpickr";
import 'flatpickr/dist/flatpickr.css';
import { format, isValid, parse } from 'date-fns'
type Props = {
  onFilter: (values: any) => void
  onReset: () => void
  openModal: () => void
  openModalExport: () => void
  linhVucs: any[]
  khois: any[]
}

const UnitsFilter: FC<Props> = ({ onFilter, openModal, openModalExport, linhVucs, khois }) => {
  const [dateState, setDateState] = useState<any>({
    date: "",
  });
  return (
    <Formik
      initialValues={{ linhVucId: '', khoiId: '', tenDonVi: '', effectiveDate: '' }}
      onSubmit={(values) => onFilter(values)}
    >
      {({ setFieldValue, values }) => (
        <Form className="row g-2 align-items-center-sm">
          {/* Select lĩnh vực */}
          <div className="col-auto">
            <Field as="select" name="linhVucId" className="form-select form-select-sm">
              <option value="">-- Chọn lĩnh vực --</option>
              {linhVucs.map((lv) => (
                <option key={lv.linhVucId} value={lv.linhVucId}>
                  {lv.tenLinhVuc}
                </option>
              ))}
            </Field>
          </div>

          {/* Select khối */}
          <div className="col-auto">
            <Field as="select" name="khoiId" className="form-select form-select-sm">
              <option value="">-- Chọn Khối --</option>
              {khois.map((k) => (
                <option key={k.khoiId} value={k.khoiId}>
                  {k.tenKhoi}
                </option>
              ))}
            </Field>
          </div>

          {/* Tên đơn vị */}
          <div className="col-auto">
            <Field
              name="tenDonVi"
              className="form-control form-control-sm"
              placeholder="Nhập tên đơn vị"
            />
          </div>

          {/* Ngày */}
          <div className="col-auto">
            <DatePicker
              selected={values.effectiveDate ? new Date(values.effectiveDate) : null}
              onChange={(date) => {
                setFieldValue(
                  "effectiveDate",
                  date ? format(date, "yyyy-MM-dd") : ""
                );
              }}
              dateFormat="dd-MM-yyyy"
              placeholderText="Pick date"
              isClearable
              className="form-control form-control-sm"
            />
          </div>

          {/* Nút */}
          <div className="col-auto d-flex align-items-center gap-2">
            <Button type="submit" className="btn btn-primary btn-sm">
              <i className="bi bi-search me-1"></i>
              Search
            </Button>
            <Button type="button" className="btn btn-info btn-sm" onClick={openModal}>
              <i className="bi bi-file-earmark-plus me-1"></i>
              Add
            </Button>
            <Button
              className="btn btn-success btn-sm"
              onClick={openModalExport}
            >
              <i className="bi bi-file-earmark-excel me-1"></i>
              Xuất Excel
            </Button>
          </div>
        </Form>
      )}
    </Formik>

  )

}

export default UnitsFilter
