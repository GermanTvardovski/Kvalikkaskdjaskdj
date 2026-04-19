--
-- PostgreSQL database dump
--

\restrict 6JFKVo9c2YftqcmHJZN3gy7BHHtd87EyM3SltxzcQIZNlxttzSzJSVodCY1iSxQ

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.6

-- Started on 2026-04-19 07:43:41

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 230 (class 1259 OID 34438)
-- Name: appointments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.appointments (
    id integer NOT NULL,
    user_id integer,
    service_id integer,
    master_id integer,
    date timestamp without time zone,
    queue_number integer
);


ALTER TABLE public.appointments OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 34437)
-- Name: appointments_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.appointments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.appointments_id_seq OWNER TO postgres;

--
-- TOC entry 4940 (class 0 OID 0)
-- Dependencies: 229
-- Name: appointments_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.appointments_id_seq OWNED BY public.appointments.id;


--
-- TOC entry 234 (class 1259 OID 34479)
-- Name: balancetransactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.balancetransactions (
    id integer NOT NULL,
    user_id integer,
    amount numeric,
    date timestamp without time zone
);


ALTER TABLE public.balancetransactions OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 34478)
-- Name: balancetransactions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.balancetransactions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.balancetransactions_id_seq OWNER TO postgres;

--
-- TOC entry 4941 (class 0 OID 0)
-- Dependencies: 233
-- Name: balancetransactions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.balancetransactions_id_seq OWNED BY public.balancetransactions.id;


--
-- TOC entry 222 (class 1259 OID 34393)
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categories (
    id integer NOT NULL,
    name character varying(100)
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 34392)
-- Name: categories_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.categories_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.categories_id_seq OWNER TO postgres;

--
-- TOC entry 4942 (class 0 OID 0)
-- Dependencies: 221
-- Name: categories_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.categories_id_seq OWNED BY public.categories.id;


--
-- TOC entry 226 (class 1259 OID 34414)
-- Name: masters; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.masters (
    id integer NOT NULL,
    name character varying(100),
    level character varying(50)
);


ALTER TABLE public.masters OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 34413)
-- Name: masters_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.masters_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.masters_id_seq OWNER TO postgres;

--
-- TOC entry 4943 (class 0 OID 0)
-- Dependencies: 225
-- Name: masters_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.masters_id_seq OWNED BY public.masters.id;


--
-- TOC entry 232 (class 1259 OID 34460)
-- Name: reviews; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reviews (
    id integer NOT NULL,
    user_id integer,
    service_id integer,
    rating integer,
    comment text
);


ALTER TABLE public.reviews OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 34459)
-- Name: reviews_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reviews_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reviews_id_seq OWNER TO postgres;

--
-- TOC entry 4944 (class 0 OID 0)
-- Dependencies: 231
-- Name: reviews_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reviews_id_seq OWNED BY public.reviews.id;


--
-- TOC entry 218 (class 1259 OID 34372)
-- Name: roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.roles (
    id integer NOT NULL,
    name character varying(50)
);


ALTER TABLE public.roles OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 34371)
-- Name: roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.roles_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.roles_id_seq OWNER TO postgres;

--
-- TOC entry 4945 (class 0 OID 0)
-- Dependencies: 217
-- Name: roles_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.roles_id_seq OWNED BY public.roles.id;


--
-- TOC entry 228 (class 1259 OID 34421)
-- Name: servicemasters; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.servicemasters (
    id integer NOT NULL,
    service_id integer,
    master_id integer
);


ALTER TABLE public.servicemasters OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 34420)
-- Name: servicemasters_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.servicemasters_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.servicemasters_id_seq OWNER TO postgres;

--
-- TOC entry 4946 (class 0 OID 0)
-- Dependencies: 227
-- Name: servicemasters_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.servicemasters_id_seq OWNED BY public.servicemasters.id;


--
-- TOC entry 224 (class 1259 OID 34400)
-- Name: services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.services (
    id integer NOT NULL,
    name character varying(100),
    description text,
    price numeric,
    category_id integer,
    updated_at timestamp without time zone
);


ALTER TABLE public.services OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 34399)
-- Name: services_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.services_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.services_id_seq OWNER TO postgres;

--
-- TOC entry 4947 (class 0 OID 0)
-- Dependencies: 223
-- Name: services_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.services_id_seq OWNED BY public.services.id;


--
-- TOC entry 220 (class 1259 OID 34379)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    name character varying(100),
    email character varying(100),
    password character varying(100),
    role_id integer,
    balance numeric
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 34378)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- TOC entry 4948 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- TOC entry 4741 (class 2604 OID 34441)
-- Name: appointments id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments ALTER COLUMN id SET DEFAULT nextval('public.appointments_id_seq'::regclass);


--
-- TOC entry 4743 (class 2604 OID 34482)
-- Name: balancetransactions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.balancetransactions ALTER COLUMN id SET DEFAULT nextval('public.balancetransactions_id_seq'::regclass);


--
-- TOC entry 4737 (class 2604 OID 34396)
-- Name: categories id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories ALTER COLUMN id SET DEFAULT nextval('public.categories_id_seq'::regclass);


--
-- TOC entry 4739 (class 2604 OID 34417)
-- Name: masters id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masters ALTER COLUMN id SET DEFAULT nextval('public.masters_id_seq'::regclass);


--
-- TOC entry 4742 (class 2604 OID 34463)
-- Name: reviews id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews ALTER COLUMN id SET DEFAULT nextval('public.reviews_id_seq'::regclass);


--
-- TOC entry 4735 (class 2604 OID 34375)
-- Name: roles id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles ALTER COLUMN id SET DEFAULT nextval('public.roles_id_seq'::regclass);


--
-- TOC entry 4740 (class 2604 OID 34424)
-- Name: servicemasters id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicemasters ALTER COLUMN id SET DEFAULT nextval('public.servicemasters_id_seq'::regclass);


--
-- TOC entry 4738 (class 2604 OID 34403)
-- Name: services id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services ALTER COLUMN id SET DEFAULT nextval('public.services_id_seq'::regclass);


--
-- TOC entry 4736 (class 2604 OID 34382)
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 4930 (class 0 OID 34438)
-- Dependencies: 230
-- Data for Name: appointments; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.appointments VALUES (1, 1, 1, 1, '2026-04-19 06:47:28.463454', 1);
INSERT INTO public.appointments VALUES (2, 2, 2, 2, '2026-04-19 06:47:28.463454', 2);
INSERT INTO public.appointments VALUES (3, 3, 3, 3, '2026-04-19 06:47:28.463454', 3);
INSERT INTO public.appointments VALUES (4, 4, 4, 4, '2026-04-19 06:47:28.463454', 4);
INSERT INTO public.appointments VALUES (5, 5, 5, 5, '2026-04-19 06:47:28.463454', 5);
INSERT INTO public.appointments VALUES (6, 6, 6, 6, '2026-04-19 06:47:28.463454', 6);
INSERT INTO public.appointments VALUES (7, 7, 7, 7, '2026-04-19 06:47:28.463454', 7);
INSERT INTO public.appointments VALUES (8, 8, 8, 8, '2026-04-19 06:47:28.463454', 8);
INSERT INTO public.appointments VALUES (9, 9, 9, 9, '2026-04-19 06:47:28.463454', 9);
INSERT INTO public.appointments VALUES (10, 10, 10, 10, '2026-04-19 06:47:28.463454', 10);


--
-- TOC entry 4934 (class 0 OID 34479)
-- Dependencies: 234
-- Data for Name: balancetransactions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.balancetransactions VALUES (1, 1, 1000, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (2, 2, 2000, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (3, 3, 500, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (4, 4, 700, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (5, 5, 300, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (6, 6, 900, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (7, 7, 1500, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (8, 8, 400, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (9, 9, 800, '2026-04-19 06:47:28.463454');
INSERT INTO public.balancetransactions VALUES (10, 10, 1200, '2026-04-19 06:47:28.463454');


--
-- TOC entry 4922 (class 0 OID 34393)
-- Dependencies: 222
-- Data for Name: categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.categories VALUES (1, 'Anime');
INSERT INTO public.categories VALUES (2, 'Новый год');
INSERT INTO public.categories VALUES (3, 'Хэллоуин');
INSERT INTO public.categories VALUES (4, 'Киберпанк');
INSERT INTO public.categories VALUES (5, 'Нуар');
INSERT INTO public.categories VALUES (6, 'Fantasy');
INSERT INTO public.categories VALUES (7, 'Horror');
INSERT INTO public.categories VALUES (8, 'Sci-Fi');
INSERT INTO public.categories VALUES (9, 'Retro');
INSERT INTO public.categories VALUES (10, 'Custom');


--
-- TOC entry 4926 (class 0 OID 34414)
-- Dependencies: 226
-- Data for Name: masters; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.masters VALUES (1, 'Alex', 'Junior');
INSERT INTO public.masters VALUES (2, 'Maria', 'Middle');
INSERT INTO public.masters VALUES (3, 'John', 'Senior');
INSERT INTO public.masters VALUES (4, 'Kate', 'Senior');
INSERT INTO public.masters VALUES (5, 'Leo', 'Junior');
INSERT INTO public.masters VALUES (6, 'Anna', 'Middle');
INSERT INTO public.masters VALUES (7, 'Max', 'Senior');
INSERT INTO public.masters VALUES (8, 'Olga', 'Middle');
INSERT INTO public.masters VALUES (9, 'Nikita', 'Junior');
INSERT INTO public.masters VALUES (10, 'Sasha', 'Senior');


--
-- TOC entry 4932 (class 0 OID 34460)
-- Dependencies: 232
-- Data for Name: reviews; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.reviews VALUES (1, 1, 1, 5, 'Отлично');
INSERT INTO public.reviews VALUES (2, 2, 2, 4, 'Хорошо');
INSERT INTO public.reviews VALUES (3, 3, 3, 3, 'Нормально');
INSERT INTO public.reviews VALUES (4, 4, 4, 5, 'Супер');
INSERT INTO public.reviews VALUES (5, 5, 5, 4, 'Хорошо');
INSERT INTO public.reviews VALUES (6, 6, 6, 5, 'Отлично');
INSERT INTO public.reviews VALUES (7, 7, 7, 3, 'Средне');
INSERT INTO public.reviews VALUES (8, 8, 8, 4, 'Хорошо');
INSERT INTO public.reviews VALUES (9, 9, 9, 5, 'Отлично');
INSERT INTO public.reviews VALUES (10, 10, 10, 5, 'Супер');


--
-- TOC entry 4918 (class 0 OID 34372)
-- Dependencies: 218
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.roles VALUES (1, 'User');
INSERT INTO public.roles VALUES (2, 'Moderator');
INSERT INTO public.roles VALUES (3, 'Admin');
INSERT INTO public.roles VALUES (4, 'Master');
INSERT INTO public.roles VALUES (5, 'Guest');
INSERT INTO public.roles VALUES (6, 'VIP');
INSERT INTO public.roles VALUES (7, 'Tester');
INSERT INTO public.roles VALUES (8, 'Manager');
INSERT INTO public.roles VALUES (9, 'Operator');
INSERT INTO public.roles VALUES (10, 'Support');


--
-- TOC entry 4928 (class 0 OID 34421)
-- Dependencies: 228
-- Data for Name: servicemasters; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.servicemasters VALUES (1, 1, 1);
INSERT INTO public.servicemasters VALUES (2, 2, 2);
INSERT INTO public.servicemasters VALUES (3, 3, 3);
INSERT INTO public.servicemasters VALUES (4, 4, 4);
INSERT INTO public.servicemasters VALUES (5, 5, 5);
INSERT INTO public.servicemasters VALUES (6, 6, 6);
INSERT INTO public.servicemasters VALUES (7, 7, 7);
INSERT INTO public.servicemasters VALUES (8, 8, 8);
INSERT INTO public.servicemasters VALUES (9, 9, 9);
INSERT INTO public.servicemasters VALUES (10, 10, 10);


--
-- TOC entry 4924 (class 0 OID 34400)
-- Dependencies: 224
-- Data for Name: services; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.services VALUES (1, 'Косплей костюм', 'Создание костюма', 5000, 1, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (2, 'Новогодний образ', 'Праздничный костюм', 3000, 2, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (3, 'Хэллоуин грим', 'Страшный макияж', 2000, 3, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (4, 'Киберпанк стиль', 'Будущее', 4000, 4, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (5, 'Нуар стиль', 'Черно-белый стиль', 3500, 5, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (6, 'Fantasy образ', 'Фэнтези', 4500, 6, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (7, 'Horror грим', 'Ужасы', 2500, 7, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (8, 'Sci-Fi костюм', 'Научная фантастика', 4800, 8, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (9, 'Retro стиль', 'Ретро', 3200, 9, '2026-04-19 06:47:28.463454');
INSERT INTO public.services VALUES (10, 'Custom заказ', 'Индивидуально', 6000, 10, '2026-04-19 06:47:28.463454');


--
-- TOC entry 4920 (class 0 OID 34379)
-- Dependencies: 220
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.users VALUES (1, 'Ivan Ivanov', 'ivan@mail.com', '123', 1, 1000);
INSERT INTO public.users VALUES (2, 'Anna Petrova', 'anna@mail.com', '123', 1, 2000);
INSERT INTO public.users VALUES (3, 'Admin Admin', 'admin@mail.com', '123', 3, 0);
INSERT INTO public.users VALUES (4, 'Moderator Mod', 'mod@mail.com', '123', 2, 0);
INSERT INTO public.users VALUES (5, 'Master One', 'master1@mail.com', '123', 4, 0);
INSERT INTO public.users VALUES (6, 'User Test', 'user1@mail.com', '123', 1, 500);
INSERT INTO public.users VALUES (7, 'User Test2', 'user2@mail.com', '123', 1, 700);
INSERT INTO public.users VALUES (8, 'User Test3', 'user3@mail.com', '123', 1, 900);
INSERT INTO public.users VALUES (9, 'User Test4', 'user4@mail.com', '123', 1, 300);
INSERT INTO public.users VALUES (10, 'User Test5', 'user5@mail.com', '123', 1, 1500);


--
-- TOC entry 4949 (class 0 OID 0)
-- Dependencies: 229
-- Name: appointments_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.appointments_id_seq', 10, true);


--
-- TOC entry 4950 (class 0 OID 0)
-- Dependencies: 233
-- Name: balancetransactions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.balancetransactions_id_seq', 10, true);


--
-- TOC entry 4951 (class 0 OID 0)
-- Dependencies: 221
-- Name: categories_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categories_id_seq', 1, false);


--
-- TOC entry 4952 (class 0 OID 0)
-- Dependencies: 225
-- Name: masters_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.masters_id_seq', 1, false);


--
-- TOC entry 4953 (class 0 OID 0)
-- Dependencies: 231
-- Name: reviews_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reviews_id_seq', 10, true);


--
-- TOC entry 4954 (class 0 OID 0)
-- Dependencies: 217
-- Name: roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.roles_id_seq', 1, false);


--
-- TOC entry 4955 (class 0 OID 0)
-- Dependencies: 227
-- Name: servicemasters_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.servicemasters_id_seq', 10, true);


--
-- TOC entry 4956 (class 0 OID 0)
-- Dependencies: 223
-- Name: services_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.services_id_seq', 1, false);


--
-- TOC entry 4957 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 1, false);


--
-- TOC entry 4757 (class 2606 OID 34443)
-- Name: appointments appointments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_pkey PRIMARY KEY (id);


--
-- TOC entry 4761 (class 2606 OID 34486)
-- Name: balancetransactions balancetransactions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.balancetransactions
    ADD CONSTRAINT balancetransactions_pkey PRIMARY KEY (id);


--
-- TOC entry 4749 (class 2606 OID 34398)
-- Name: categories categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (id);


--
-- TOC entry 4753 (class 2606 OID 34419)
-- Name: masters masters_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.masters
    ADD CONSTRAINT masters_pkey PRIMARY KEY (id);


--
-- TOC entry 4759 (class 2606 OID 34467)
-- Name: reviews reviews_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_pkey PRIMARY KEY (id);


--
-- TOC entry 4745 (class 2606 OID 34377)
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id);


--
-- TOC entry 4755 (class 2606 OID 34426)
-- Name: servicemasters servicemasters_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicemasters
    ADD CONSTRAINT servicemasters_pkey PRIMARY KEY (id);


--
-- TOC entry 4751 (class 2606 OID 34407)
-- Name: services services_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_pkey PRIMARY KEY (id);


--
-- TOC entry 4747 (class 2606 OID 34386)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 4766 (class 2606 OID 34454)
-- Name: appointments appointments_master_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_master_id_fkey FOREIGN KEY (master_id) REFERENCES public.masters(id);


--
-- TOC entry 4767 (class 2606 OID 34449)
-- Name: appointments appointments_service_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_service_id_fkey FOREIGN KEY (service_id) REFERENCES public.services(id);


--
-- TOC entry 4768 (class 2606 OID 34444)
-- Name: appointments appointments_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.appointments
    ADD CONSTRAINT appointments_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- TOC entry 4771 (class 2606 OID 34487)
-- Name: balancetransactions balancetransactions_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.balancetransactions
    ADD CONSTRAINT balancetransactions_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- TOC entry 4769 (class 2606 OID 34473)
-- Name: reviews reviews_service_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_service_id_fkey FOREIGN KEY (service_id) REFERENCES public.services(id);


--
-- TOC entry 4770 (class 2606 OID 34468)
-- Name: reviews reviews_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reviews
    ADD CONSTRAINT reviews_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


--
-- TOC entry 4764 (class 2606 OID 34432)
-- Name: servicemasters servicemasters_master_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicemasters
    ADD CONSTRAINT servicemasters_master_id_fkey FOREIGN KEY (master_id) REFERENCES public.masters(id);


--
-- TOC entry 4765 (class 2606 OID 34427)
-- Name: servicemasters servicemasters_service_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.servicemasters
    ADD CONSTRAINT servicemasters_service_id_fkey FOREIGN KEY (service_id) REFERENCES public.services(id);


--
-- TOC entry 4763 (class 2606 OID 34408)
-- Name: services services_category_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.services
    ADD CONSTRAINT services_category_id_fkey FOREIGN KEY (category_id) REFERENCES public.categories(id);


--
-- TOC entry 4762 (class 2606 OID 34387)
-- Name: users users_role_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_role_id_fkey FOREIGN KEY (role_id) REFERENCES public.roles(id);


-- Completed on 2026-04-19 07:43:41

--
-- PostgreSQL database dump complete
--

\unrestrict 6JFKVo9c2YftqcmHJZN3gy7BHHtd87EyM3SltxzcQIZNlxttzSzJSVodCY1iSxQ

